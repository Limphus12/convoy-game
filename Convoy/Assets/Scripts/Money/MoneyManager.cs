using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using com.limphus.save_system;

namespace com.limphus.convoy
{
    public class MoneyManager : MonoBehaviour
    {
        private const int BASE_VEHICLE_COST = 1000;
        private const float PER_VEHICLE_MULTI = 1.1f;

        protected static int currentMoney;

        public static event EventHandler<Events.IntEventArgs> OnMoneyChangedEvent;
        public static event EventHandler<EventArgs> OnVehiclePurchasedEvent, OnVehicleSoldEvent;

        protected static void OnMoneyChanged() => OnMoneyChangedEvent?.Invoke(typeof(MoneyManager), new Events.IntEventArgs { i = currentMoney });
        protected static void OnVehiclePurchased() => OnVehiclePurchasedEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);
        protected static void OnVehicleSold() => OnVehicleSoldEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);

        public static int GetCurrentMoney() => currentMoney;

        public static bool CanRemoveMoney(int amount) => currentMoney >= amount;

        public static void SetCurrentMoney(int amount) { currentMoney = amount; OnMoneyChanged(); }
        public static void AddMoney(int amount) { currentMoney += amount; OnMoneyChanged(); }
        public static void RemoveMoney(int amount) { currentMoney -= amount; OnMoneyChanged(); }

        public static int GetCurrentVehicleCost()
        {
            return BASE_VEHICLE_COST * ConvoyManager.vehiclesList.Count;
        }

        public static int GetCurrentVehicleSellPrice()
        {
            return BASE_VEHICLE_COST * (ConvoyManager.vehiclesList.Count - 1);
        }

        private void Awake()
        {
            SaveSystem.OnGameLoadedEvent += SaveSystem_OnGameLoadedEvent;
            Shop.OnVehiclePurchasedEvent += Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleSoldEvent += Shop_OnVehicleSoldEvent;
        }

        private void OnDestroy()
        {
            SaveSystem.OnGameLoadedEvent -= SaveSystem_OnGameLoadedEvent;
            Shop.OnVehiclePurchasedEvent -= Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleSoldEvent -= Shop_OnVehicleSoldEvent;
        }

        private void Shop_OnVehiclePurchasedEvent(object sender, EventArgs e)
        {
            RemoveMoney(GetCurrentVehicleCost());

            OnVehiclePurchased();
        }

        private void Shop_OnVehicleSoldEvent(object sender, EventArgs e)
        {
            AddMoney(GetCurrentVehicleSellPrice());

            OnVehicleSold();
        }

        private void SaveSystem_OnGameLoadedEvent(object sender, SaveSystemEvents.OnGameChangedEventArgs e)
        {
            SetCurrentMoney(e.i.money);
        }
    }
}