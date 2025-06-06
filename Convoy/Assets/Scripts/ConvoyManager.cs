using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using com.limphus.save_system;
using com.limphus.utilities;
using PathCreation;

namespace com.limphus.convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        [SerializeField] private GameObject vehiclePrefab;

        public static List<Vehicle> vehiclesList = new List<Vehicle>();

        public Vector3[] vehiclePositions;
        public PathCreator[] vehiclePaths;

        public static int currentVehicleIndex;

        public static Vehicle currentVehicle, previousVehicle;

        private void Awake()
        {
            vehiclesList.Clear();
            currentVehicleIndex = 0;
            currentVehicle = null;

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

                vh.ChassisManager.SetPart(e.i.vehicleDatas[i].chassisIndex);
                vh.ChassisManager.TurretManager.SetPart(e.i.vehicleDatas[i].turretIndex);

                vh.ChassisManager.hasSpawnedFirstPart = true;
                vh.ChassisManager.TurretManager.hasSpawnedFirstPart = true;

                Target tg = vhobj.GetComponentInChildren<Target>(); 
                if (tg) tg.OnDeathEvent += Target_OnDeathEvent;
            }

            currentVehicle = vehiclesList[currentVehicleIndex];
        }

        private void Target_OnDeathEvent(object sender, Events.GameObjectEventArgs e)
        {
            //make sure to remove the vehicle from our list when it dies!
            vehiclesList.Remove(e.i.GetComponentInParent<Vehicle>());

            e.i.GetComponentInChildren<Target>().OnDeathEvent -= Target_OnDeathEvent;
        }

        private void Start()
        {
            SetVehiclePositions();
        }

        private void SetVehiclePositions()
        {
            for (int i = 0; i < vehiclesList.Count; i++)
            {
                if (vehiclePositions[i] == null || vehiclesList[i] == null) return;

                vehiclesList[i].transform.localPosition = vehiclePositions[i];
            }
        }

        public static void SwitchVehicle(bool forward)
        {
            if (vehiclesList.Count == 0) return;

            previousVehicle = currentVehicle;

            //increment or decrement the part index
            if (forward && currentVehicleIndex < vehiclesList.Count - 1) currentVehicleIndex++;
            else if (!forward && currentVehicleIndex > 0) currentVehicleIndex--;

            //assign the current vehicle from the vehicle index
            currentVehicle = vehiclesList[currentVehicleIndex];
        }
    }
}