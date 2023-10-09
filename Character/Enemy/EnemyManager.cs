using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.AI;

namespace AP
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        [SerializeField] Animator anim;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;
        public EnemyInventoryManager enemyInventoryManager;
        public EnemyEquipmentManager enemyEquipmentManager;
        public EnemyCombatManager enemyCombatManager;
        public EnemyEffectsManager enemyEffectsManager;
        EnemyNetworkManager enemyNetworkManager;
        NetworkObject networkObject;

        CharacterManager characterManager;

        public State currentState;
        public IdleState idleState;
        public CharacterManager currentTarget;

        public float distanceFromTarget;
        public float rotationSpeed = 200f;
        public float maximumAttackRange = 1.5f;
        public float runningSpeed = 1;

        [Header("A.I Settings")]
        public float detectionRadius = 20;

        // The higher, and lower, respectively these angles are, the greater the detection FIELD OF VIEW (sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float viewableAngle;

        public float currentRecoveryTime = 0;

        [SerializeField] bool processDeathEvent = false;

        int oldHP;

        protected override void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyRigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
            enemyInventoryManager = GetComponent<EnemyInventoryManager>();
            enemyEquipmentManager = GetComponent<EnemyEquipmentManager>();
            enemyCombatManager = GetComponent<EnemyCombatManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyNetworkManager = GetComponent<EnemyNetworkManager>();
            networkObject = GetComponent<NetworkObject>();

            oldHP = enemyNetworkManager.maxHealth.Value;

            networkObject.Spawn();

            characterManager = GetComponent<CharacterManager>();
            

            Vector3 enemySpawnLocation = transform.position - transform.position;
            enemySpawnLocation.x = 50.73342f;
            enemySpawnLocation.y = 20f;
            enemySpawnLocation.z = 62.51769f;
            transform.position = enemySpawnLocation;

            
        }

        protected override void Start()
        {
            base.Start();

            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;

            enemyNetworkManager.vitality.Value = 10;
        }
            

        protected override void Update()
        {
            base.Update();

            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            if (currentTarget != null)
            {
                if (currentTarget.isDead.Value == true)
                {
                    currentTarget = null;
                }
            }

            DebugMenu();



            anim = GetComponent<Animator>();
            enemyCombatManager = GetComponent<EnemyCombatManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();

            HandleRecoveryTimer();

            if (enemyNetworkManager.currentHealth != oldHP)
            {
                enemyNetworkManager.CheckHP(oldHP, enemyNetworkManager.currentHealth);
                oldHP = enemyNetworkManager.currentHealth;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            enemyNetworkManager.vitality.OnValueChanged += enemyNetworkManager.SetNewMaxHealthValue;

        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            float timer = 3;

            if (IsOwner)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendYouWinPopUp();

                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            // CHECK FOR PLAYERS THAT ARE ALIVE, IF 0 RESPAWN CHARACTERS

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public void DebugMenu()
        {
            if (processDeathEvent == true)
            {
                processDeathEvent = false;

                StartCoroutine(ProcessDeathEvent(false));
            }
        }
    }
}