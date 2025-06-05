using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    [Serializable]
    public struct VFXData
    {
        public GameObject particles;
        public float time;
    }

    public class AITurretVFX : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private Transform firePoint;

        [Space]
        [SerializeField] private VFXData muzzleVFX;
        [SerializeField] private VFXData bulletVFX;

        [Space]
        [SerializeField] private VFXData hitParticles;
        [SerializeField] private Vector3 hitOffset;

        private AITurret aiTurret;

        private void Awake()
        {
            aiTurret = GetComponent<AITurret>();

            if (aiTurret)
            {
                aiTurret.OnStartAttackEvent += AiTurret_OnStartAttackEvent;
                aiTurret.OnHitEvent += AiTurret_OnHitEvent;
            }
        }

        private void OnDestroy()
        {
            if (aiTurret)
            {
                aiTurret.OnStartAttackEvent -= AiTurret_OnStartAttackEvent;
                aiTurret.OnHitEvent -= AiTurret_OnHitEvent;
            }
        }

        private void AiTurret_OnStartAttackEvent(object sender, EventArgs e)
        {
            ShootingVFX();
        }

        private void ShootingVFX()
        {
            if (muzzleVFX.particles)
            {
                GameObject particles = Instantiate(muzzleVFX.particles, firePoint.position, firePoint.rotation, firePoint);
                Destroy(particles, muzzleVFX.time);
            }

            if (bulletVFX.particles)
            {
                GameObject particles = Instantiate(bulletVFX.particles, firePoint.position, firePoint.rotation);
                Destroy(particles, bulletVFX.time);
            }
        }

        private void AiTurret_OnHitEvent(object sender, Events.Vector3EventArgs e)
        {
            HitVFX(e.i);
        }

        private void HitVFX(Vector3 pos)
        {
            if (hitParticles.particles)
            {
                GameObject particles = Instantiate(hitParticles.particles, pos + hitOffset, Quaternion.identity);
                Destroy(particles, hitParticles.time);
            }
        }
    }
}