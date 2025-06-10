using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class AITurretAnimation : AnimationHandler
    {
        private AITurret aiTurret;
        [SerializeField] private float firingAnimResetTime = 1f;

        private void Awake()
        {
            aiTurret = GetComponent<AITurret>();

            if (aiTurret)
            {
                aiTurret.OnStartAttackEvent += AiTurret_OnStartAttackEvent;
            }
        }

        private void OnDestroy()
        {
            if (aiTurret)
            {
                aiTurret.OnStartAttackEvent -= AiTurret_OnStartAttackEvent;
            }
        }

        private void AiTurret_OnStartAttackEvent(object sender, EventArgs e)
        {
            SetParamater("IsFiring", true);

            Invoke(nameof(ResetFiring), firingAnimResetTime);
        }

        void ResetFiring()
        {
            SetParamater("IsFiring", false);
        }
    }
}