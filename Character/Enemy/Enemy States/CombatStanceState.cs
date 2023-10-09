using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public IdleState idleState;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.currentTarget != null)
            {
                // Check for attack range
                enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                // Potentially circle player or walk around them

            
                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
                {
                    // if in attack range, return attack state
                    return attackState;
                }
                else if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
                {
                    // if the player runs out of range, return the pursue target state
                    return pursueTargetState;
                }
                else
                {
                    // if we are in a cool down after attacking, return this state and continue circling player
                    return this;
                }
            }
            else
            {
                return idleState;
            }
        }
    }
}