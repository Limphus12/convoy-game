using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class ConvoyCustomisationCamera : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 1f;
        [SerializeField] private Transform cmvTarget;

        private Vector3 prevPos, nextPos, origPos;

        private void Awake()
        {
            origPos = cmvTarget.position;
            prevPos = origPos;

            ConvoyManager.OnVehicleSwitchedEvent += ConvoyManager_OnVehicleSwitchedEvent;
            ConvoyManager.OnVehicleRemovedEvent += ConvoyManager_OnVehicleRemovedEvent;
        }

        private void OnDestroy()
        {
            ConvoyManager.OnVehicleSwitchedEvent -= ConvoyManager_OnVehicleSwitchedEvent;
            ConvoyManager.OnVehicleRemovedEvent -= ConvoyManager_OnVehicleRemovedEvent;
        }

        private void ConvoyManager_OnVehicleSwitchedEvent(object sender, EventArgs e)
        {
            //when a vehicle is switched, we simply move the target backwards or forwards
            SetCameraTarget(ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex].transform.position);
        }

        private void ConvoyManager_OnVehicleRemovedEvent(object sender, EventArgs e)
        {
            //if there is only one vehicle, move to it
            if (ConvoyManager.vehiclesList.Count > 0) SetCameraTarget(ConvoyManager.currentVehicle.transform.position);

            //if no vehicles, send to our original pos
            else if (ConvoyManager.vehiclesList.Count == 0) SetCameraTarget(origPos);
        }

        protected void SetCameraTarget(Vector3 pos)
        {
            StopCoroutine(SmoothCameraSwitch());

            nextPos = pos;

            StartCoroutine(SmoothCameraSwitch());
        }

        public void SwitchCameraTarget(bool forward)
        {
            StopCoroutine(SmoothCameraSwitch());

            //if (ConvoyManager.vehiclesList.Count == 0) return;
            //if (ConvoyManager.currentVehicleIndex == 0 && !forward) return;
            //if (ConvoyManager.currentVehicleIndex == ConvoyManager.vehiclesList.Count - 1 && forward) return;
            //
            //if (ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex] != null) prevPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex].transform.position;
            //
            //if (forward && ConvoyManager.currentVehicleIndex < ConvoyManager.vehiclesList.Count - 1) nextPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex /+ /1].transform.position;
            //
            //else if (!forward && ConvoyManager.currentVehicleIndex > 0) nextPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex - 1].transform.position;

            StartCoroutine(SmoothCameraSwitch());
        }

        IEnumerator SmoothCameraSwitch()
        {
            float i = 0;

            while (i < 1)
            {
                cmvTarget.position = Vector3.Lerp(prevPos, nextPos, i);

                yield return new WaitForEndOfFrame();

                i += smoothSpeed * Time.deltaTime;
            }

            cmvTarget.position = nextPos;
            prevPos = nextPos;
        }
    }
}