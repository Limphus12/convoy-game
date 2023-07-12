using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class AITurret : Turret
    {
        [Header("AI Targeting")]
        [SerializeField] private float attackRange;
        [SerializeField] private float updateInterval;

        private Transform currentTarget;
        private Target[] targets;

        private void Start()
        {
            InvokeRepeating(nameof(FindTargets), 0f, updateInterval);
        }

        private void Update()
        {
            if (currentTarget != null) CalculateRotation(currentTarget.position);
        }

        private void FindTargets()
        {
            targets = FindObjectsOfType<Target>();

            if (targets.Length == 0) return;

            float nearestDistance = float.MaxValue;

            foreach (Target target in targets)
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    currentTarget = target.transform;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}