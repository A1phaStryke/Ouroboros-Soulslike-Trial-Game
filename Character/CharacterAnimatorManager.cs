using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace AP 
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        public Animator anim;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            anim = GetComponent<Animator>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void Update()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting = false)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;


            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation,
                                                      bool isPerformingAction, 
                                                      bool applyRootMotion = true, 
                                                      bool canRotate = false, 
                                                      bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS WHILE THEY ALREADY ARE PERFORMING ONE
            // FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
            // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
            // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING A NEW ACTION
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYONE PRESENT
            character.characterNetworkManager.NotifyTheServerOfAnActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
                                                      string targetAnimation,
                                                      bool isPerformingAction, 
                                                      bool applyRootMotion = true, 
                                                      bool canRotate = false, 
                                                      bool canMove = false)
        {
            // KEEP TRACK OF LAST ATTACK PERFORMED (FOR COMBOS)
            // KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY, ETC)
            // UPDATE ANIMATION SET TO CURRENT WEAPONS ANIMATIONS
            // DECIDE IF OUR ATTACK CAN BE PARRIED
            // TELL THE NETWORK OUR "ISATTACKING" FLAG IS ACTIVE (For counter damage)
            character.characterCombatManager.currentAttackType = attackType;
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYONE PRESENT
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetEnemyActionAnimation(
                                                      string targetAnimation,
                                                      bool isPerformingAction, 
                                                      bool applyRootMotion = true, 
                                                      bool canRotate = false, 
                                                      bool canMove = false)
        {
            // KEEP TRACK OF LAST ATTACK PERFORMED (FOR COMBOS)
            // KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY, ETC)
            // UPDATE ANIMATION SET TO CURRENT WEAPONS ANIMATIONS
            // DECIDE IF OUR ATTACK CAN BE PARRIED
            // TELL THE NETWORK OUR "ISATTACKING" FLAG IS ACTIVE (For counter damage)
            character.applyRootMotion = applyRootMotion;
            anim.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYONE PRESENT
            //character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
