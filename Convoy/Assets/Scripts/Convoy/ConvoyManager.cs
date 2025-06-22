using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using com.limphus.save_system;
using com.limphus.utilities;
using BezierSolution;

namespace com.limphus.convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        [SerializeField] private GameObject vehiclePrefab;
        [SerializeField] private float vehicleSeperation = 10f;
        [SerializeField] private Vector3 initVehiclePos;
        [SerializeField] private BezierSpline spline;

        public static float currentVehicleSeperation;

        public static List<Vehicle> vehiclesList = new List<Vehicle>();

        public static int currentVehicleIndex;

        public static Vehicle currentVehicle;

        public static event EventHandler<EventArgs> OnVehicleAddedEvent, OnVehicleRemovedEvent, OnVehicleStartRemovedEvent, OnVehicleStartSwitchedEvent, OnVehicleSwitchedEvent;

        protected void OnVehicleAdded() => OnVehicleAddedEvent?.Invoke(this, EventArgs.Empty);

        protected void OnVehicleStartRemoved() => OnVehicleStartRemovedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleRemoved() => OnVehicleRemovedEvent?.Invoke(this, EventArgs.Empty);

        protected void OnVehicleSwitched() => OnVehicleSwitchedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleStartSwitched() => OnVehicleStartSwitchedEvent?.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            vehiclesList.Clear();
            currentVehicleIndex = 0;
            currentVehicle = null;

            currentVehicleSeperation = vehicleSeperation;

            SaveSystem.OnConvoyLoadedEvent += SaveSystem_OnConvoyLoadedEvent;
            MoneyManager.OnVehiclePurchasedEvent += MoneyManager_OnVehiclePurchasedEvent;
            MoneyManager.OnVehicleSoldEvent += MoneyManager_OnVehicleSoldEvent;
        }

        private void OnDestroy()
        {
            SaveSystem.OnConvoyLoadedEvent -= SaveSystem_OnConvoyLoadedEvent;
            Shop.OnVehiclePurchasedEvent -= MoneyManager_OnVehiclePurchasedEvent;
            Shop.OnVehicleSoldEvent -= MoneyManager_OnVehicleSoldEvent;
        }

        private void SaveSystem_OnConvoyLoadedEvent(object sender, SaveSystemEvents.OnConvoyChangedEventArgs e)
        {
            if (e.i.vehicleDatas.Count == 0) return;

            for (int i = 0; i < e.i.vehicleDatas.Count; i++)
            {
                GameObject vhobj; 

                if (i == 0) vhobj = Instantiate(vehiclePrefab, initVehiclePos, Quaternion.identity, transform);
                else vhobj = Instantiate(vehiclePrefab, new Vector3(0, 0, (i * -vehicleSeperation)), Quaternion.identity, transform);

                Vehicle vh = vhobj.GetComponent<Vehicle>();

                if (vh != null)
                {
                    vehiclesList.Add(vh);

                    if (i == 0)
                    {
                        //only set the beizer to the first vehicle
                        if (spline) vh.SetBeizer(spline);
                        vh.isFirstVehicle = true;
                    }

                    vh.GetChassisManager().SetPart(e.i.vehicleDatas[i].chassisIndex);
                    vh.ChassisManager.GetTurretManager().SetPart(e.i.vehicleDatas[i].turretIndex);

                    vh.ChassisManager.hasSpawnedFirstPart = true;
                    vh.ChassisManager.TurretManager.hasSpawnedFirstPart = true;

                    Target tg = vhobj.GetComponentInChildren<Target>();
                    if (tg) tg.OnDeathEvent += Target_OnDeathEvent;
                }

                else Debug.Log("No Vehicle Found!");
            }

            if (vehiclesList[currentVehicleIndex] != null) currentVehicle = vehiclesList[currentVehicleIndex];
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

                else vehiclesList[i].transform.localPosition = initVehiclePos + (Vector3.back * (i * vehicleSeperation));
            }
        }

        public static void SetVehicle(int index)
        {
            if (vehiclesList.Count == 0) return;

            currentVehicleIndex = index;

            //assign the current vehicle from the vehicle index
            currentVehicle = vehiclesList[currentVehicleIndex];
        }

        public void SwitchVehicle(bool forward)
        {
            if (vehiclesList.Count == 0) return;

            OnVehicleStartSwitched();

            //increment or decrement the part index
            if (forward && currentVehicleIndex < vehiclesList.Count - 1) currentVehicleIndex++;
            else if (!forward && currentVehicleIndex > 0) currentVehicleIndex--;

            //assign the current vehicle from the vehicle index
            currentVehicle = vehiclesList[currentVehicleIndex];

            OnVehicleSwitched();
        }

        public void AddVehicle()
        {
            GameObject vhobj = Instantiate(vehiclePrefab, new Vector3(0, 0, (vehiclesList.Count + 1) * -vehicleSeperation), Quaternion.identity, transform);

            Vehicle vh = vhobj.GetComponent<Vehicle>();

            if (vh != null)
            {
                vehiclesList.Add(vh);

                vh.GetChassisManager().SetPart(0);
                vh.ChassisManager.GetTurretManager().SetPart(0);

                vh.ChassisManager.hasSpawnedFirstPart = true;
                vh.ChassisManager.TurretManager.hasSpawnedFirstPart = true;

                Target tg = vhobj.GetComponentInChildren<Target>();
                if (tg) tg.OnDeathEvent += Target_OnDeathEvent;

                OnVehicleAdded();
            }

            else Debug.Log("Trying to add Vehicle... No Vehicle Found!");
        }

        public void RemoveVehicle()
        {
            //(later on we'll need to get back the parts)

            //destroy the current vehicle and remove it from the list
            if (currentVehicle == null) return;

            OnVehicleStartRemoved();

            Destroy(currentVehicle.gameObject);
            currentVehicle = null;
            vehiclesList.RemoveAt(currentVehicleIndex); vehiclesList.TrimExcess();

            if (vehiclesList.Count == 0) { OnVehicleRemoved(); return; }

            //now select a new vehicle

            if (currentVehicleIndex == 0) SetVehicle(currentVehicleIndex);
            else SetVehicle(currentVehicleIndex - 1);

            //set the vehicle positions
            SetVehiclePositions();

            OnVehicleRemoved();
        }

        private void MoneyManager_OnVehicleSoldEvent(object sender, EventArgs e)
        {
            RemoveVehicle();
        }

        private void MoneyManager_OnVehiclePurchasedEvent(object sender, EventArgs e)
        {
            AddVehicle();
        }
    }
}