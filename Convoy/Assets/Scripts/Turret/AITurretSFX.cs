using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class AITurretSFX : MonoBehaviour
    {
        [Header("SFX")]
        [SerializeField] SoundDataStruct turretAttackSounds;

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
            SFX();
        }

        private void SFX()
        {
            SoundHandler.PlaySound(turretAttackSounds, null, transform.position);
        }
    }
}