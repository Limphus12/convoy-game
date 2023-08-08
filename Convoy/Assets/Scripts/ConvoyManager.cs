using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace com.limphus.convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        [SerializeField] private Vehicle[] vehicles;

        [Space]
        [SerializeField] private Vector3[] vehiclePositions;

        [Space] [SerializeField] private CinemachineVirtualCamera cmvCam;

        [Header("UI")]
        [SerializeField] private Button chassisSwitchForwardButton;
        [SerializeField] private Button chassisSwitchBackButton;

        [Space]
        [SerializeField] private Button turretSwitchForwardButton;
        [SerializeField] private Button turretSwitchBackButton;

        private int currentVehicleIndex;
        private Vehicle currentVehicle;

        private void Awake()
        {
            currentVehicleIndex = 0;

            if (vehicles.Length == 0)
            {
                vehicles = gameObject.GetComponentsInChildren<Vehicle>();
            }
        }

        private void Start()
        {
            if (vehicles.Length == 0)
            {
                vehicles = gameObject.GetComponentsInChildren<Vehicle>();
            }

            if (vehicles.Length != 0) currentVehicle = vehicles[0];

            SetVehiclePositions(); ButtonSetup();
        }

        private void Update()
        {
            Inputs();
        }

        private void Inputs()
        {

        }

        private void SetVehiclePositions()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehiclePositions[i] == null) return;

                vehicles[i].transform.position = vehiclePositions[i];
            }
        }

        public void SwitchForward() => SwitchVehicle(true);

        public void SwitchBackward() => SwitchVehicle(false);

        private void SwitchVehicle(bool forward)
        {
            if (vehicles.Length == 0) return;

            // Increment or decrement the part index based on the 'forward' flag and loop back to the start/end if necessary
            //if (forward) CurrentVehicle = (CurrentVehicle + 1) % vehicles.Length;
            //else CurrentVehicle = (CurrentVehicle - 1 + vehicles.Length) % vehicles.Length;

            //increment or decrement the part index based on the 'forward' boolean
            if (forward && currentVehicleIndex < vehicles.Length - 1) currentVehicleIndex++;
            else if (!forward && currentVehicleIndex > 0) currentVehicleIndex--;

            //assign the current vehicle from the vehicle index
            currentVehicle = vehicles[currentVehicleIndex];

            ButtonSetup();

            if (cmvCam) SwitchCameraTarget();
        }

        private void ButtonSetup()
        {
            //remove and reassign buttons for the chassis and turret managers
            chassisSwitchForwardButton.onClick.RemoveAllListeners();
            chassisSwitchBackButton.onClick.RemoveAllListeners();
            turretSwitchForwardButton.onClick.RemoveAllListeners();
            turretSwitchBackButton.onClick.RemoveAllListeners();

            chassisSwitchForwardButton.onClick.AddListener(currentVehicle.ChassisManager.SwitchForward);
            chassisSwitchBackButton.onClick.AddListener(currentVehicle.ChassisManager.SwitchBackward);
            turretSwitchForwardButton.onClick.AddListener(currentVehicle.TurretManager.SwitchForward);
            turretSwitchBackButton.onClick.AddListener(currentVehicle.TurretManager.SwitchBackward);
        }

        private void SwitchCameraTarget()
        {
            cmvCam.LookAt = vehicles[currentVehicleIndex].transform;
            cmvCam.Follow = vehicles[currentVehicleIndex].transform;
        }
    }
}