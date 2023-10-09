using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace AP
{
    public class EnemyNetworkManager : CharacterNetworkManager
    {
        EnemyManager enemy;

        protected override void Awake()
        {
            base.Awake();

            
            enemy = GetComponent<EnemyManager>();
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = enemy.enemyStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);

            currentHealth = maxHealth.Value;
        }
    }
}