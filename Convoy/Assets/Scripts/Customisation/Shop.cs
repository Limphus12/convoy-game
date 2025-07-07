using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class Shop : MonoBehaviour
    {
        public static event EventHandler<EventArgs> OnVehiclePurchasedEvent, OnVehicleNotPurchasedEvent, OnVehicleSoldEvent, OnVehicleNotSoldEvent;
        public static event EventHandler<EventArgs> OnChassisUpgradedEvent, OnChassisNotUpgradedEvent, OnTurretUpgradedEvent, OnTurretNotUpgradedEvent;

        protected void OnVehiclePurchased() => OnVehiclePurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleNotPurchased() => OnVehicleNotPurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleSold() => OnVehicleSoldEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleNotSold() => OnVehicleNotSoldEvent?.Invoke(this, EventArgs.Empty);


        protected void OnChassisUpgraded() => OnChassisUpgradedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnChassisNotUpgraded() => OnChassisNotUpgradedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnTurretUpgraded() => OnTurretUpgradedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnTurretNotUpgraded() => OnTurretNotUpgradedEvent?.Invoke(this, EventArgs.Empty);


        public static event EventHandler<EventArgs> OnVehicleFullyRepairedEvent, OnVehiclePartiallyRepairedEvent, OnVehicleNotRepairedEvent;

        protected void OnVehicleFullyRepaired() => OnVehicleFullyRepairedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehiclePartiallyRepaired() => OnVehiclePartiallyRepairedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleNotRepaired() => OnVehicleNotRepairedEvent?.Invoke(this, EventArgs.Empty);


        private void Awake()
        {
            UIManager.OnVehiclePurchasedButtonEvent += UIManager_OnVehiclePurchasedButtonEvent;
            UIManager.OnVehicleSoldButtonEvent += UIManager_OnVehicleSoldButtonEvent;

            UIManager.OnChassisUpgradedButtonEvent += UIManager_OnChassisUpgradedButtonEvent;
            UIManager.OnTurretUpgradedButtonEvent += UIManager_OnTurretupgradedButtonEvent;

            UIManager.OnVehicleRepairedButtonEvent += UIManager_OnVehicleRepairedButtonEvent;
        }

        private void OnDestroy()
        {
            UIManager.OnVehiclePurchasedButtonEvent -= UIManager_OnVehiclePurchasedButtonEvent;
            UIManager.OnVehicleSoldButtonEvent -= UIManager_OnVehicleSoldButtonEvent;

            UIManager.OnChassisUpgradedButtonEvent -= UIManager_OnChassisUpgradedButtonEvent;
            UIManager.OnTurretUpgradedButtonEvent -= UIManager_OnTurretupgradedButtonEvent;

            UIManager.OnVehicleRepairedButtonEvent -= UIManager_OnVehicleRepairedButtonEvent;
        }

        private void UIManager_OnVehiclePurchasedButtonEvent(object sender, EventArgs e)
        {
            //TODO: add check for vehicle maximum amount; we don't want to go over that

            //validate the purchase; if we have the money, then tell the convoy manager to add a vehicle!
            if (MoneyManager.CanRemoveMoney(MoneyManager.GetCurrentVehicleCost())) OnVehiclePurchased();

            else OnVehicleNotPurchased();
        }

        private void UIManager_OnVehicleSoldButtonEvent(object sender, EventArgs e)
        {
            //validate the sale; if we're not down to 1 vehicle, sell the vehicle!
            if (ConvoyManager.vehiclesList.Count > 1) OnVehicleSold();

            else OnVehicleNotSold();
        }

        private void UIManager_OnChassisUpgradedButtonEvent(object sender, EventArgs e)
        {
            //validate the purchase!
            if (MoneyManager.CanRemoveMoney(MoneyManager.GetCurrentVehicleChassisUpgradeCost())) OnChassisUpgraded();

            else OnChassisNotUpgraded();
        }

        private void UIManager_OnTurretupgradedButtonEvent(object sender, EventArgs e)
        {
            //validate the purchase!
            if (MoneyManager.CanRemoveMoney(MoneyManager.GetCurrentVehicleTurretUpgradeCost())) OnTurretUpgraded();

            else OnTurretNotUpgraded();
        }

        private void UIManager_OnVehicleRepairedButtonEvent(object sender, EventArgs e)
        {
            Target tg = ConvoyManager.currentVehicle.Target;

            if (tg.GetCurrentHealth() < tg.GetMaxHealth())
            {
                if (MoneyManager.CanRemoveMoney(MoneyManager.GetCurrentVehicleFullRepairCost())) OnVehicleFullyRepaired();
                else if (MoneyManager.CanRemoveMoney(MoneyManager.GetCurrentVehiclePartialRepairCost())) OnVehiclePartiallyRepaired();
            }

            else OnVehicleNotRepaired();
        }
    }
}