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
            Debug.Log("Part Changed! Resetting Buttons");
        
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
            if (ConvoyManager.currentVehicle == null) { Debug.LogWarning("Current Vehicle is Null!"); return; }

            ChassisButtonSetup();
            TurretButtonSetup();
        }

        private void ButtonReset()
        {
            if (ConvoyManager.currentVehicle == null) { Debug.LogWarning("Current Vehicle is Null!"); return; }

            ChassisButtonReset();
            TurretButtonReset();
        }

        public void ChassisButtonReset()
        {
            if (ConvoyManager.currentVehicle.GetChassisManager() == null) Debug.Log("Cannot Reset Chassis Buttons!");
            else Debug.Log(ConvoyManager.currentVehicle.GetChassisManager() + ": Button Reset");

            chassisSwitchForwardButton.onClick.RemoveListener(ConvoyManager.currentVehicle.GetChassisManager().SwitchForward);
            chassisSwitchBackButton.onClick.RemoveListener(ConvoyManager.currentVehicle.GetChassisManager().SwitchBackward);
        }

        public void TurretButtonReset()
        {
            if (ConvoyManager.currentVehicle.ChassisManager.GetTurretManager() == null) Debug.Log("Cannot Reset Turret Buttons!");
            Debug.Log(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager() + ": Button Reset");

            turretSwitchForwardButton.onClick.RemoveListener(ConvoyManager.currentVehicle.GetChassisManager().GetTurretManager().SwitchForward);
            turretSwitchBackButton.onClick.RemoveListener(ConvoyManager.currentVehicle.GetChassisManager().GetTurretManager().SwitchBackward);
        }

        public void ChassisButtonSetup()
        {
            Debug.Log(ConvoyManager.currentVehicle.GetChassisManager() + ": Button Setup");

            chassisSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.GetChassisManager().SwitchForward);
            chassisSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.GetChassisManager().SwitchBackward);
        }

        public void TurretButtonSetup()
        {
            Debug.Log(ConvoyManager.currentVehicle.ChassisManager.GetTurretManager() + ": Button Setup");

            turretSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.GetChassisManager().GetTurretManager().SwitchForward);
            turretSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.GetChassisManager().GetTurretManager().SwitchBackward);
        }
    }
}