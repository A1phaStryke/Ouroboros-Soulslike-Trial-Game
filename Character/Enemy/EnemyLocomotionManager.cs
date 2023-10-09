using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AP
{
    public class EnemyLocomotionManager : CharacterLocomotionManager
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        public float distanceFromTarget;

        protected override void Awake()
        {
            base.Awake();

            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        


        
    }
}