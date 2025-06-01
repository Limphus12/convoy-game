using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class ConvoyCustomisationCamera : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 1f;
        [SerializeField] private Transform cmvTarget;

        private Vector3 prevPos;
        private Vector3 nextPos;

        public void SwitchCameraTarget(bool forward)
        {
            StopCoroutine(SmoothCameraSwitch());

            prevPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex].transform.position;

            if (forward && ConvoyManager.currentVehicleIndex < ConvoyManager.vehiclesList.Count - 1) nextPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex + 1].transform.position;

            else if (!forward && ConvoyManager.currentVehicleIndex > 0) nextPos = ConvoyManager.vehiclesList[ConvoyManager.currentVehicleIndex - 1].transform.position;

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
        }
    }
}