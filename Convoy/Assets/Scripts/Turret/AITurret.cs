using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class AITurret : Turret
    {
        [Header("AI Targeting")]
        [SerializeField] private float targetingInterval;
        [SerializeField] private float attackRange;
        [SerializeField] private bool needLOS;

        [Header("AI Shooting")]
        [SerializeField] private float fireRate;
        [SerializeField] private int damage;

        [Header("VFX")]
        [SerializeField] private bool vfx;

        [Space, SerializeField] private Transform firePoint;

        [Space]
        [SerializeField] private GameObject muzzleParticles;
        [SerializeField] private GameObject bulletParticles;

        [Header("SFX")]
        [SerializeField] private AudioSource audioSource;

        [Space]
        [SerializeField] private AudioClip shootingClip;

        private bool isAttacking;

        private Target currentTarget;

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

            //if the player has selected a target, just focus on that
            if (TargetSystem.playerSelectedTarget != null)
            {
                currentTarget = TargetSystem.playerSelectedTarget; return;
            }

            //TODO: Line of sight check, to ensure we cannot shoot over hills.

            float nearestDistance = float.MaxValue;

            foreach (Target target in TargetSystem.targetArray)
            {
                if (target.IsDead()) continue;

                float distance = Vector3.Distance(target.transform.position, transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;

                    if (nearestDistance <= attackRange) currentTarget = target;
                }
            }
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
            Hit(); Debug.Log(name + "is firing at " + currentTarget.name + "!");

            VFX(); SFX();
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
            if (!vfx) return;

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