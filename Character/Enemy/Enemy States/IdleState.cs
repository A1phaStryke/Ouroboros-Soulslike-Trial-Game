using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        
        public LayerMask detectionLayer;
        
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            // Look for a potential target
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].transform.GetComponent<CharacterManager>();

                if (character != null)
                {
                    // CHECK FOR TEAM ID

                    Vector3 targetDirection = character.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = character;
                    }
                }
            }
            #endregion

            // Switch to pursue target state if target is found
            // if not, return this state

            #region Handle Switching To Next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}