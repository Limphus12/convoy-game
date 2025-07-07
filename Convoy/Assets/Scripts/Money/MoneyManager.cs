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

        private const int BASE_VEHICLE_REPAIR_COST = 10;

        protected static int currentMoney;

        public static event EventHandler<Events.IntEventArgs> OnMoneyChangedEvent;
        public static event EventHandler<EventArgs> OnVehiclePurchasedEvent, OnVehicleSoldEvent, OnChassisUpgradedEvent, OnTurretUpgradedEvent;
        public static event EventHandler<Events.IntEventArgs> OnVehicleFullyRepairedEvent, OnVehiclePartiallyRepairedEvent;

        protected static void OnMoneyChanged() => OnMoneyChangedEvent?.Invoke(typeof(MoneyManager), new Events.IntEventArgs { i = currentMoney });
        protected static void OnVehiclePurchased() => OnVehiclePurchasedEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);
        protected static void OnVehicleSold() => OnVehicleSoldEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);
        protected static void OnVehicleFullyRepaired(int amount) => OnVehicleFullyRepairedEvent?.Invoke(typeof(MoneyManager), new Events.IntEventArgs { i = amount });
        protected static void OnVehiclePartiallyRepaired(int amount) => OnVehiclePartiallyRepairedEvent?.Invoke(typeof(MoneyManager), new Events.IntEventArgs { i = amount });
        protected static void OnChassisUpgraded() => OnChassisUpgradedEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);
        protected static void OnTurretUpgraded() => OnTurretUpgradedEvent?.Invoke(typeof(MoneyManager), EventArgs.Empty);

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

        public static int GetCurrentVehicleFullRepairCost()
        {
            Target tg = ConvoyManager.currentVehicle.Target;
            int maxHealth = tg.GetMaxHealth(), currentHealth = tg.GetCurrentHealth();

            return BASE_VEHICLE_REPAIR_COST * (maxHealth - currentHealth);
        }

        public static int GetCurrentVehiclePartialRepairCost()
        {
            return GetCurrentMoney();
        }

        public static int GetCurrentVehicleChassisUpgradeCost()
        {
            int chassisIndex = ConvoyManager.currentVehicle.ChassisManager.GetCurrentPartIndex();

            if (chassisIndex == 0) return 1000;
            if (chassisIndex == 1) return 2500;
            if (chassisIndex == 2) return 5000;

            else return 10000;
        }

        public static int GetCurrentVehicleTurretUpgradeCost()
        {
            int turretIndex = ConvoyManager.currentVehicle.ChassisManager.TurretManager.GetCurrentPartIndex();

            if (turretIndex == 0) return 1000;
            if (turretIndex == 1) return 2500;
            if (turretIndex == 2) return 5000;

            else return 10000;
        }

        private void Awake()
        {
            SaveSystem.OnGameLoadedEvent += SaveSystem_OnGameLoadedEvent;
            Shop.OnVehiclePurchasedEvent += Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleSoldEvent += Shop_OnVehicleSoldEvent;
            Shop.OnVehicleFullyRepairedEvent += Shop_OnVehicleFullyRepairedEvent;
            Shop.OnVehiclePartiallyRepairedEvent += Shop_OnVehiclePartiallyRepairedEvent;
            Shop.OnChassisUpgradedEvent += Shop_OnChassisUpgradedEvent;
            Shop.OnTurretUpgradedEvent += Shop_OnTurretUpgradedEvent;
        }

        private void OnDestroy()
        {
            SaveSystem.OnGameLoadedEvent -= SaveSystem_OnGameLoadedEvent;
            Shop.OnVehiclePurchasedEvent -= Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleSoldEvent -= Shop_OnVehicleSoldEvent;
            Shop.OnVehicleFullyRepairedEvent -= Shop_OnVehicleFullyRepairedEvent;
            Shop.OnVehiclePartiallyRepairedEvent -= Shop_OnVehiclePartiallyRepairedEvent;
            Shop.OnChassisUpgradedEvent -= Shop_OnChassisUpgradedEvent;
            Shop.OnTurretUpgradedEvent -= Shop_OnTurretUpgradedEvent;
        }

        private void Shop_OnChassisUpgradedEvent(object sender, EventArgs e)
        {
            RemoveMoney(GetCurrentVehicleChassisUpgradeCost());

            OnChassisUpgraded();
        }

        private void Shop_OnTurretUpgradedEvent(object sender, EventArgs e)
        {
            RemoveMoney(GetCurrentVehicleTurretUpgradeCost());

            OnTurretUpgraded();
        }

        private void Shop_OnVehicleFullyRepairedEvent(object sender, EventArgs e)
        {
            int amount = GetCurrentVehicleFullRepairCost();

            RemoveMoney(amount);

            OnVehicleFullyRepaired(amount);
        }

        private void Shop_OnVehiclePartiallyRepairedEvent(object sender, EventArgs e)
        {
            int amount = GetCurrentVehiclePartialRepairCost();

            RemoveMoney(amount);

            OnVehiclePartiallyRepaired(amount);
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