using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemy;
        EnemyNetworkManager enemyNetworkManager;

        protected override void Awake()
        {
            base.Awake();

            enemy = GetComponent<EnemyManager>();
            enemyNetworkManager = GetComponent<EnemyNetworkManager>();
        }

        protected override void Start()
        {
            base.Start();

            CalculateHealthBasedOnVitalityLevel(enemyNetworkManager.vitality.Value);
        }
    }
}