using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public enum TargetPriority { CLOSE, FAR, STRONG, WEAK }

    public class AITurret : Turret
    {
        [Header("AI Targeting")]
        [SerializeField] private TargetType targetType;
        [SerializeField] private TargetPriority targetPriority;

        [Space]
        [SerializeField] private float targetingInterval;
        [SerializeField] private bool useRange;
        [SerializeField] private float attackRange;

        [Space]
        [SerializeField] private bool needLOS;
        [SerializeField] private LayerMask layer;


        [Header("AI Shooting")]
        [SerializeField] private float fireRate;
        [SerializeField] private int damage;

        [Header("VFX")]
        [SerializeField] private Transform firePoint;

        [Space]
        [SerializeField] private GameObject muzzleParticles;
        [SerializeField] private GameObject bulletParticles;

        [Header("SFX")]
        [SerializeField] private AudioSource audioSource;

        [Space]
        [SerializeField] private AudioClip shootingClip;

        private bool isAttacking;

        private Target currentTarget;

        public int GetDamage() => damage;

        private void Start()
        {
            InvokeRepeating(nameof(FindTargets), 0f, targetingInterval);
        }

        private void Update()
        {
            if (currentTarget != null) CalculateRotation(currentTarget.transform.position);

            CheckTargets();
        }

        private void CheckTargets()
        {
            if (currentTarget == null) return;

            if (!isAttacking && !currentTarget.IsDead()) StartAttack();
        }

        private void FindTargets()
        {
            if (!TargetSystem.HasTargets()) return;

            //if the player has selected a target and we're in range, just focus on that
            if (TargetSystem.playerSelectedTarget != null)
            {
                float distance = Vector3.Distance(TargetSystem.playerSelectedTarget.transform.position, transform.position);

                if (distance <= attackRange)
                {
                    currentTarget = TargetSystem.playerSelectedTarget; return;
                }
            }

            //TODO: Line of sight check, to ensure we cannot shoot over hills.

            //TODO: target priority - closest, furthest, toughest, weakest.
            //we're gonna do toughest and weakest based on the damage it can do, not the health.

            List<Target> targets;

            switch (targetType)
            {
                case TargetType.Player: targets = TargetSystem.playerTargets; break;

                case TargetType.Enemy: targets = TargetSystem.enemyTargets; break;

                default: targets = TargetSystem.enemyTargets; break;
            }

            switch (targetPriority)
            {
                case TargetPriority.CLOSE:

                    float nearestDistance = float.MaxValue;

                    //loop through the targets and find the closest one within attack range.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead() || target == GetComponent<Target>()) continue;

                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;

                            if (useRange && nearestDistance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.FAR:

                    float furthestDistance = float.MinValue;

                    // Loop through the targets and find the furthest one within attack range.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead()) continue;

                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance > furthestDistance)
                        {
                            furthestDistance = distance;

                            if (useRange && distance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.STRONG:

                    //int highestDamage = int.MinValue;
                    //
                    ////loop through the targets and find the strongest one, health-wise.
                    //foreach (Target target in targets)
                    //{
                    //    if (target.IsDead()) continue;
                    //
                    //    int damage = target.GetCurrentHealth();
                    //
                    //    if (damage > highestDamage)
                    //    {
                    //        highestDamage = damage;
                    //        currentTarget = target;
                    //    }
                    //}

                    break;
                case TargetPriority.WEAK:

                    break;

                default:
                    break;
            }
        }


        private bool HasLOS(Target target)
        {
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, layer))
            {
                Debug.Log(hit.transform.gameObject.name);

                return hit.transform == target.transform;
            }

            else return false;
        }

        private void StartAttack()
        {
            isAttacking = true;

            Attack();

            //invoke end shoot after our rate of fire
            Invoke(nameof(EndAttack), 1 / fireRate);
        }

        private void Attack()
        {
            //call the hit function
            Hit(); VFX(); SFX();
        }

        private void SFX()
        {
            if (audioSource)
            {
                audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.1f);

                audioSource.PlayOneShot(shootingClip);
            }
        }

        private void VFX()
        {
            if (muzzleParticles)
            {
                GameObject particles = Instantiate(muzzleParticles, firePoint.position, firePoint.rotation, firePoint);
                Destroy(particles, 1f);
            }

            if (bulletParticles)
            {
                GameObject particles = Instantiate(bulletParticles, firePoint.position, firePoint.rotation);
                Destroy(particles, 5f);
            }
        }

        private void EndAttack() => isAttacking = false;

        private void Hit()
        {
            IDamageable damageable = currentTarget.GetComponent<IDamageable>();

            if (damageable != null) damageable.Damage(damage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}