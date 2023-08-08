using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace com.limphus.convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        [SerializeField] private Vehicle[] vehicles;

        [Space]
        [SerializeField] private Vector3[] vehiclePositions;

        [Space] [SerializeField] private CinemachineVirtualCamera cmvCam;

        public int CurrentVehicle { get; private set; }

        private void Awake()
        {
            CurrentVehicle = 0;

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

            SetVehiclePositions();
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
            if (forward && CurrentVehicle < vehicles.Length - 1) CurrentVehicle++;
            else if (!forward && CurrentVehicle > 0) CurrentVehicle--;

            if (cmvCam) SwitchCameraTarget();
        }

        private void SwitchCameraTarget()
        {
            cmvCam.LookAt = vehicles[CurrentVehicle].transform;
            cmvCam.Follow = vehicles[CurrentVehicle].transform;
        }
    }
}