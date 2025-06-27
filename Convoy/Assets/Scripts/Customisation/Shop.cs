using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class Shop : MonoBehaviour
    {
        public static event EventHandler<EventArgs> OnVehiclePurchasedEvent, OnVehicleNotPurchasedEvent, OnVehicleSoldEvent, OnVehicleNotSoldEvent;
        public static event EventHandler<EventArgs> OnChassisPurchasedEvent, OnChassisNotPurchasedEvent, OnChassisSoldEvent, OnChassisNotSoldEvent;

        protected void OnVehiclePurchased() => OnVehiclePurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleNotPurchased() => OnVehicleNotPurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleSold() => OnVehicleSoldEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVehicleNotSold() => OnVehicleNotSoldEvent?.Invoke(this, EventArgs.Empty);


        protected void OnChassisPurchased() => OnChassisPurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnChassisNotPurchased() => OnChassisNotPurchasedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnChassisSold() => OnChassisSoldEvent?.Invoke(this, EventArgs.Empty);
        protected void OnChassisNotSold() => OnChassisNotSoldEvent?.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            UIManager.OnVehiclePurchasedButtonEvent += UIManager_OnVehiclePurchasedButtonEvent;
            UIManager.OnVehicleSoldButtonEvent += UIManager_OnVehicleSoldButtonEvent;

            UIManager.OnChassisPurchasedButtonEvent += UIManager_OnChassisPurchasedButtonEvent;
            UIManager.OnChassisSoldButtonEvent += UIManager_OnChassisSoldButtonEvent;
        }

        private void OnDestroy()
        {
            UIManager.OnVehiclePurchasedButtonEvent -= UIManager_OnVehiclePurchasedButtonEvent;
            UIManager.OnVehicleSoldButtonEvent -= UIManager_OnVehicleSoldButtonEvent;
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

        private void UIManager_OnChassisPurchasedButtonEvent(object sender, EventArgs e)
        {
            //we need to get the specific part price... and check the cost against our money

            OnChassisPurchased();
        }

        private void UIManager_OnChassisSoldButtonEvent(object sender, EventArgs e)
        {
            //we'd need to get the specific part price for this too...

            OnChassisSold();
        }
    }
}