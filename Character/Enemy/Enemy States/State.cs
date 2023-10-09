using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager);
    }
}