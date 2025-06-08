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
        [SerializeField] private float vehicleSeperation = 10f;
        [SerializeField] private Vector3 initVehiclePos;

        public static List<Vehicle> vehiclesList = new List<Vehicle>();

        public PathCreator[] vehiclePaths;

        public static int currentVehicleIndex;

        public static Vehicle currentVehicle;

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
                GameObject vhobj; 

                if (i == 0) vhobj = Instantiate(vehiclePrefab, initVehiclePos, Quaternion.identity, transform);
                else vhobj = Instantiate(vehiclePrefab, new Vector3(0, 0, (i * -vehicleSeperation)), Quaternion.identity, transform);

                Vehicle vh = vhobj.GetComponent<Vehicle>();

                if (vh != null)
                {
                    vehiclesList.Add(vh);

                    //if (vehiclePaths.Length != 0 && vehiclePaths[i] != null) vh.SetPath(vehiclePaths[i]);

                    vh.GetChassisManager().SetPart(e.i.vehicleDatas[i].chassisIndex);
                    vh.ChassisManager.GetTurretManager().SetPart(e.i.vehicleDatas[i].turretIndex);

                    vh.ChassisManager.hasSpawnedFirstPart = true;
                    vh.ChassisManager.TurretManager.hasSpawnedFirstPart = true;

                    Target tg = vhobj.GetComponentInChildren<Target>();
                    if (tg) tg.OnDeathEvent += Target_OnDeathEvent;
                }

                else Debug.Log("No Vehicle Found!");
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
                if (i == 0) vehiclesList[i].transform.localPosition = initVehiclePos;

                else vehiclesList[i].transform.localPosition = new Vector3(0, 0, (i * -vehicleSeperation) + initVehiclePos.z);
            }
        }

        public static void SwitchVehicle(bool forward)
        {
            if (vehiclesList.Count == 0) return;

            //increment or decrement the part index
            if (forward && currentVehicleIndex < vehiclesList.Count - 1) currentVehicleIndex++;
            else if (!forward && currentVehicleIndex > 0) currentVehicleIndex--;

            //assign the current vehicle from the vehicle index
            currentVehicle = vehiclesList[currentVehicleIndex];
        }

        public void AddVehicle()
        {
            GameObject vhobj = Instantiate(vehiclePrefab, new Vector3(0, 0, (vehiclesList.Count + 1) * -vehicleSeperation), Quaternion.identity, transform);

            Vehicle vh = vhobj.GetComponent<Vehicle>();

            if (vh != null)
            {
                vehiclesList.Add(vh);

                if (vehiclePaths.Length != 0) vh.SetPath(vehiclePaths[currentVehicleIndex + 1]);

                vh.GetChassisManager().SetPart(0);
                vh.ChassisManager.GetTurretManager().SetPart(0);

                vh.ChassisManager.hasSpawnedFirstPart = true;
                vh.ChassisManager.TurretManager.hasSpawnedFirstPart = true;

                Target tg = vhobj.GetComponentInChildren<Target>();
                if (tg) tg.OnDeathEvent += Target_OnDeathEvent;
            }

            else Debug.Log("Trying to add Vehicle... No Vehicle Found!");
        }

        public static void RemoveVehicle(int index)
        {
            //we need to move and then shift all the vehicles in our convoy

            //later on we'll need to get back the parts
        }
    }
}