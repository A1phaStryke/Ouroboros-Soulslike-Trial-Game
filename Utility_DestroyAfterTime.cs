using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroyed = 5;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}
