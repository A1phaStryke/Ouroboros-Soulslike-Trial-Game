using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class EnemyEquipmentManager : CharacterEquipmentManager
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        public WeaponModelInstantiationSlot rightHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        
        public GameObject rightHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();

            // GET OUR SLOTS
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadRightWeapon();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadRightWeapon()
        {
            if (enemyManager.enemyInventoryManager.currentRightHandWeapon != null)
            {
                // REMOVE OLD WEAPON
                rightHandSlot.UnloadWeapon();

                // CREATE NEW WEAPON
                rightHandWeaponModel = Instantiate(enemyManager.enemyInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);

                // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(enemyManager, enemyManager.enemyInventoryManager.currentRightHandWeapon);
            }
        } 

        public void OpenDamageCollider()
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();

            // PLAY SFX
        }

        public void CloseDamageCollider()
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }
}