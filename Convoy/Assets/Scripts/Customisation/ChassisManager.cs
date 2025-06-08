using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class ChassisManager : PartManager
    {
        public TurretManager TurretManager { get; private set; }

        public static event EventHandler<EventArgs> OnChassisChangedEvent;

        protected void OnChassisChanged() => OnChassisChangedEvent?.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            OnPartChangedEvent += ChassisManager_OnPartChangedEvent;

            FindTurretManager();
        }

        private void OnDestroy()
        {
            OnPartChangedEvent -= ChassisManager_OnPartChangedEvent;
        }

        private void ChassisManager_OnPartChangedEvent(object sender, EventArgs e)
        {
            FindTurretManager();
        }

        public void FindTurretManager()
        {
            TurretManager = GetComponentInChildren<TurretManager>();
        }

        public TurretManager GetTurretManager()
        {
            FindTurretManager();

            if (TurretManager) return TurretManager;

            else return null;
        }

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

            OnChassisChanged();
        }
    }
}