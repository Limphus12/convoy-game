using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class TurretManager : PartManager
    {
        public static event EventHandler<EventArgs> OnTurretChangedEvent;

        protected void OnTurretChanged() => OnTurretChangedEvent?.Invoke(this, EventArgs.Empty);

        private void Start()
        {
            if (!hasSpawnedFirstPart)
            {
                SpawnPart();
                hasSpawnedFirstPart = true;
            }
        }

        protected override void SpawnPart()
        {
            base.SpawnPart();

            OnTurretChanged();
        }
    }
}