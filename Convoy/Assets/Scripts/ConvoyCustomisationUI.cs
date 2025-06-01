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

        [SerializeField] private Button turretSwitchForwardButton;
        [SerializeField] private Button turretSwitchBackButton;

        private void Start()
        {
            ButtonSetup();
        }

        public void SwitchForward() => SwitchVehicle(true);

        public void SwitchBackward() => SwitchVehicle(false);

        private void SwitchVehicle(bool forward)
        {
            if (ConvoyManager.vehiclesList.Count == 0) return;

            //increment or decrement the part index
            if (forward && ConvoyManager.currentVehicleIndex < ConvoyManager.vehiclesList.Count - 1) ConvoyManager.currentVehicleIndex++;
            else if (!forward && ConvoyManager.currentVehicleIndex > 0) ConvoyManager.currentVehicleIndex--;

            //assign the current vehicle from the vehicle index
            ConvoyManager.currentVehicle = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex];

            ButtonSetup();
        }

        private void ButtonSetup()
        {
            //remove and reassign buttons for the chassis and turret managers
            chassisSwitchForwardButton.onClick.RemoveAllListeners();
            chassisSwitchBackButton.onClick.RemoveAllListeners();
            turretSwitchForwardButton.onClick.RemoveAllListeners();
            turretSwitchBackButton.onClick.RemoveAllListeners();

            if (ConvoyManager.currentVehicle == null) return;

            chassisSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.SwitchForward);
            chassisSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.ChassisManager.SwitchBackward);
            turretSwitchForwardButton.onClick.AddListener(ConvoyManager.currentVehicle.TurretManager.SwitchForward);
            turretSwitchBackButton.onClick.AddListener(ConvoyManager.currentVehicle.TurretManager.SwitchBackward);
        }
    }
}