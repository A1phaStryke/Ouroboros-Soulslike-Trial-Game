using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;  // (When calculatung damage this is used to check for attackers damage modifiers, effects, etc)
        [SerializeField] CharacterCombatManager characterCombatManager;

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }

            damageCollider.enabled = false; // MELEE WEAPON COLLIDERS SHOULD BE DISABLED AT START, ONLY ENABLED WHEN ANIMATIONS ALLOW

        }

        private void Update()
        {
            if (characterCombatManager == null)
            {
                characterCombatManager = GetComponentInParent<CharacterCombatManager>();
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                // WE DO NOT WANT TO DAMAGE OURSELVES
                if (damageTarget == characterCausingDamage)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE

                // CHECK IF TARGET IS BLOCKING

                // CHECK IF TARGET IS INVULNERABLE

                // DAMAGE
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // WE DONT WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            // SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE

            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.contactPoint = contactPoint;

            Debug.Log("damageEffect: " + damageEffect);
            Debug.Log("currentAttackType: " + characterCombatManager.currentAttackType);
            Debug.Log("light_Attack_01_Modifier: " + light_Attack_01_Modifier);

            switch (characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    break;
                default:
                    break;
            }

            if (characterCausingDamage.IsOwner)
            {
                // SEND A DAMAGE REQUEST ACROSS THE NETWORK
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, 
                                                                                               characterCausingDamage.NetworkObjectId,
                                                                                               damageEffect.physicalDamage,
                                                                                               damageEffect.magicDamage,
                                                                                               damageEffect.fireDamage,
                                                                                               damageEffect.holyDamage,
                                                                                               damageEffect.lightningDamage,
                                                                                               damageEffect.poiseDamage,
                                                                                               damageEffect.angleHitFrom,
                                                                                               damageEffect.contactPoint.x,
                                                                                               damageEffect.contactPoint.y,
                                                                                               damageEffect.contactPoint.z);
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.poiseDamage *= modifier;

            // IF ATTACK IS FULLY CHARGED HEAVY, MULTIPLY BY FULL CHARGE MODIFIER AFTER NORMAL MODIFIER HAVE BEEN CALCULATED
        }
    }
}
