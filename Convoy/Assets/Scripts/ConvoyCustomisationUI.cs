using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class ConvoyCustomisationUI : MonoBehaviour
    {
        [SerializeField] private Button chassisSwitchForwardButton;
        [SerializeField] private Button chassisSwitchBackButton;

        [Space]
        [SerializeField] private Button turretSwitchForwardButton;
        [SerializeField] private Button turretSwitchBackButton;

        private void Awake()
        {
            PartManager.OnPartChangedEvent += PartManager_OnPartChangedEvent;
        }

        private void PartManager_OnPartChangedEvent(object sender, EventArgs e)
        {
            ButtonReset();
            ButtonSetup();
        }

        private void OnDestroy()
        {
            PartManager.OnPartChangedEvent -= PartManager_OnPartChangedEvent;
        }

        private void Start()
        {
            ButtonSetup();
        }

        public void SwitchForward() => SwitchVehicle(true);

        public void SwitchBackward() => SwitchVehicle(false);

        private void SwitchVehicle(bool forward)
        {
            ButtonReset();

            ConvoyManager.SwitchVehicle(forward);

            ButtonSetup();
        }
        private void ButtonSetup()
        {
            if (ConvoyManager.currentVehicle == null) return;

            ChassisButtonSetup();
            TurretButtonSetup();
        }

        private void ButtonReset()
        {
            if (ConvoyManager.currentVehicle == null) return;

            ChassisButtonReset();
            TurretButtonReset();
        }

        public void ChassisButtonReset()
        {
            chassisSwitchForwardButton.onClick.RemoveListener(ConvoyManager.currentVehicle.ChassisManager.SwitchForward);
            chassisSwitchBackButton.onClick.RemoveListener(ConvoyManager.currentVehicle.ChassisManager.SwitchBackward);
        }

        public void TurretButtonReset()
        {
            turretSwitchForwardButton.onClick.RemoveListener(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager().SwitchForward);
            turretSwitchBackButton.onClick.RemoveListener(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager().SwitchBackward);
        }

        public void ChassisButtonSetup()
        {
            chassisSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.SwitchForward);
            chassisSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.SwitchBackward);
        }

        public void TurretButtonSetup()
        {
            turretSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager().SwitchForward);
            turretSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager().SwitchBackward);
        }
    }
}