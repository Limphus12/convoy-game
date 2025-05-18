using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class AITurretVFX : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private Transform firePoint;

        [Space]
        [SerializeField] private GameObject muzzleParticles;
        [SerializeField] private GameObject bulletParticles;

        private AITurret aiTurret;

        void Start()
        {
            aiTurret = GetComponent<AITurret>();

            if (aiTurret)
            {
                aiTurret.OnStartAttackEvent += AiTurret_OnStartAttackEvent;
            }
        }

        private void AiTurret_OnStartAttackEvent(object sender, EventArgs e)
        {
            VFX();
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
                Destroy(particles, 3f);
            }
        }
    }
}