using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using com.limphus.save_system;
using PathCreation;

namespace com.limphus.convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        public static List<Vehicle> vehiclesList = new List<Vehicle>();

        public Vector3[] vehiclePositions;
        public PathCreator[] vehiclePaths;

        public static int currentVehicleIndex;
        public static Vehicle currentVehicle;

        [SerializeField] private GameObject vehiclePrefab;

        private void Awake()
        {
            SaveSystem.OnConvoyLoadedEvent += SaveSystem_OnConvoyLoadedEvent;
        }

        private void OnDestroy()
        {
            SaveSystem.OnConvoyLoadedEvent -= SaveSystem_OnConvoyLoadedEvent;
        }

        private void SaveSystem_OnConvoyLoadedEvent(object sender, SaveSystemEvents.OnConvoyChangedEventArgs e)
        {
            for (int i = 0; i < e.i.vehicleDatas.Count; i++)
            {
                GameObject vhobj = Instantiate(vehiclePrefab, vehiclePositions[i], Quaternion.identity, transform);

                Vehicle vh = vhobj.GetComponent<Vehicle>();

                vehiclesList.Add(vh);

                if (vehiclePaths.Length != 0) vh.SetPath(vehiclePaths[i]);

                vh.ChassisManager.SetPartIndex(e.i.vehicleDatas[i].chassisIndex);
                vh.TurretManager.SetPartIndex(e.i.vehicleDatas[i].turretIndex);
            }

            currentVehicle = vehiclesList[0];
        }

        private void Start()
        {
            SetVehiclePositions();
        }

        private void SetVehiclePositions()
        {
            for (int i = 0; i < vehiclesList.Count; i++)
            {
                if (vehiclePositions[i] == null) return;

                vehiclesList[i].transform.localPosition = vehiclePositions[i];
            }
        }
    }
}