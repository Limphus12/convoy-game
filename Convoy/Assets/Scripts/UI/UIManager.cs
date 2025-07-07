using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using com.limphus.save_system;
using UnityEngine.UI;
using TMPro;

namespace com.limphus.convoy
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteUI, levelFailUI, gameUI, menuUI, pauseUI;

        [Space]
        [SerializeField] private GameObject optionsMenu, controlsPanel, videoPanel, audioPanel;

        [Space]
        [SerializeField] private GameObject playerTargetUI;
        [SerializeField] private HealthBar playerTargetHealthUI;

        [Space]
        [SerializeField] private GameObject enemyTargetUI;
        [SerializeField] private HealthBar enemyTargetHealthUI;

        [Space]
        [SerializeField] private TextMeshProUGUI altitudeTextUI;
        [SerializeField] private TextMeshProUGUI speedTextUI, timerTextUI, headingTextUI, verticalTextUI, moneyTextUI;

        [Space]
        [SerializeField] private GameObject purchaseUI;
        [SerializeField] private GameObject sellUI, purchaseFailUI, saleFailUI;

        [Space]
        [SerializeField] private GameObject repairUI;
        [SerializeField] private GameObject repairFailUI;

        [Space]
        [SerializeField] private GameObject chassisUpgradeUI;
        [SerializeField] private GameObject chassisUpgradeFailUI;

        [Space]
        [SerializeField] private GameObject turretUpgradeUI;
        [SerializeField] private GameObject turretUpgradeFailUI;

        public static event EventHandler<Events.BoolEventArgs> OnFlirToggledEvent;
        public static event EventHandler<EventArgs> OnCameraToggledEvent, OnVehiclePurchasedButtonEvent, OnVehicleSoldButtonEvent, OnChassisUpgradedButtonEvent, OnTurretUpgradedButtonEvent, OnVehicleRepairedButtonEvent;

        protected void OnFlirToggled(bool b) => OnFlirToggledEvent.Invoke(this, new Events.BoolEventArgs { i = b });
        protected void OnCameraToggled() => OnCameraToggledEvent.Invoke(this, EventArgs.Empty);
        protected void OnVehiclePurchasedButton() => OnVehiclePurchasedButtonEvent.Invoke(this, EventArgs.Empty);
        protected void OnVehicleSoldButton() => OnVehicleSoldButtonEvent.Invoke(this, EventArgs.Empty);
        protected void OnChassisUpgradedButton() => OnChassisUpgradedButtonEvent.Invoke(this, EventArgs.Empty);
        protected void OnTurretUpgradedButton() => OnTurretUpgradedButtonEvent.Invoke(this, EventArgs.Empty);
        protected void OnVehicleRepairedButton() => OnVehicleRepairedButtonEvent.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            GameManager.OnLevelCompleteEvent += GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent += GameManager_OnLevelFailEvent;

            PauseManager.OnPausedChangedEvent += PauseManager_OnPausedChangedEvent;

            TargetSystem.OnPlayerTargetSelectedEvent += TargetSystem_OnPlayerTargetSelectedEvent;
            TargetSystem.OnPlayerTargetDeSelectedEvent += TargetSystem_OnPlayerTargetDeSelectedEvent;

            TargetSystem.OnEnemyTargetSelectedEvent += TargetSystem_OnEnemyTargetSelectedEvent;
            TargetSystem.OnEnemyTargetDeSelectedEvent += TargetSystem_OnEnemyTargetDeSelectedEvent;

            MoneyManager.OnMoneyChangedEvent += MoneyManager_OnMoneyChangedEvent;

            Shop.OnVehiclePurchasedEvent += Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleNotPurchasedEvent += Shop_OnVehicleNotPurchasedEvent;

            Shop.OnVehicleSoldEvent += Shop_OnVehicleSoldEvent;
            Shop.OnVehicleNotSoldEvent += Shop_OnVehicleNotSoldEvent;

            Shop.OnVehicleFullyRepairedEvent += Shop_OnVehicleFullyRepairedEvent;
            Shop.OnVehiclePartiallyRepairedEvent += Shop_OnVehiclePartiallyRepairedEvent;
            Shop.OnVehicleNotRepairedEvent += Shop_OnVehicleNotRepairedEvent;

            Shop.OnChassisUpgradedEvent += Shop_OnChassisUpgradedEvent;
            Shop.OnChassisNotUpgradedEvent += Shop_OnChassisNotUpgradedEvent;

            Shop.OnTurretUpgradedEvent += Shop_OnTurretUpgradedEvent;
            Shop.OnTurretNotUpgradedEvent += Shop_OnTurretNotUpgradedEvent;

            InvokeRepeating(nameof(RandomAltitudeUI), 0f, .5f);
            InvokeRepeating(nameof(RandomSpeedUI), 0f, .5f);
        }

        private void OnDestroy()
        {
            GameManager.OnLevelCompleteEvent -= GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent -= GameManager_OnLevelFailEvent;

            PauseManager.OnPausedChangedEvent -= PauseManager_OnPausedChangedEvent;

            TargetSystem.OnPlayerTargetSelectedEvent -= TargetSystem_OnPlayerTargetSelectedEvent;
            TargetSystem.OnPlayerTargetDeSelectedEvent -= TargetSystem_OnPlayerTargetDeSelectedEvent;

            TargetSystem.OnEnemyTargetSelectedEvent -= TargetSystem_OnEnemyTargetSelectedEvent;
            TargetSystem.OnEnemyTargetDeSelectedEvent -= TargetSystem_OnEnemyTargetDeSelectedEvent;

            MoneyManager.OnMoneyChangedEvent -= MoneyManager_OnMoneyChangedEvent;

            Shop.OnVehiclePurchasedEvent -= Shop_OnVehiclePurchasedEvent;
            Shop.OnVehicleNotPurchasedEvent -= Shop_OnVehicleNotPurchasedEvent;

            Shop.OnVehicleSoldEvent -= Shop_OnVehicleSoldEvent;
            Shop.OnVehicleNotSoldEvent -= Shop_OnVehicleNotSoldEvent;

            Shop.OnVehicleFullyRepairedEvent -= Shop_OnVehicleFullyRepairedEvent;
            Shop.OnVehiclePartiallyRepairedEvent -= Shop_OnVehiclePartiallyRepairedEvent;
            Shop.OnVehicleNotRepairedEvent -= Shop_OnVehicleNotRepairedEvent;
        }

        #region EnemyTargetSelection

        private void TargetSystem_OnEnemyTargetSelectedEvent(object sender, EventArgs e)
        {
            //when we have an enemy target selected, enable the pov cam
            if (enemyTargetUI) enemyTargetUI.SetActive(true);

            if (enemyTargetHealthUI)
            {
                enemyTargetHealthUI.SetMaxValue(TargetSystem.enemySelectedTarget.GetMaxHealth());
                enemyTargetHealthUI.SetCurrentValue(TargetSystem.enemySelectedTarget.GetCurrentHealth());
            }

            TargetSystem.enemySelectedTarget.OnHealthChangedEvent += EnemySelectedTarget_OnHealthChangedEvent;
            TargetSystem.enemySelectedTarget.OnDeathEvent += EnemySelectedTarget_OnDeathEvent;
        }

        private void TargetSystem_OnEnemyTargetDeSelectedEvent(object sender, EventArgs e)
        {
            //when we have an enemy target get deselected, disable
            if (enemyTargetUI) enemyTargetUI.SetActive(false);

            TargetSystem.enemySelectedTarget.OnHealthChangedEvent -= EnemySelectedTarget_OnHealthChangedEvent;
            TargetSystem.enemySelectedTarget.OnDeathEvent -= EnemySelectedTarget_OnDeathEvent;
        }

        private void EnemySelectedTarget_OnHealthChangedEvent(object sender, EventArgs e)
        {
            if (enemyTargetHealthUI)
            {
                enemyTargetHealthUI.SetCurrentValue(TargetSystem.enemySelectedTarget.GetCurrentHealth());
            }
        }

        private void EnemySelectedTarget_OnDeathEvent(object sender, Events.GameObjectEventArgs e)
        {
            //when we have an enemy target get killed, disable
            if (enemyTargetUI) enemyTargetUI.SetActive(false);

            TargetSystem.enemySelectedTarget.OnHealthChangedEvent -= EnemySelectedTarget_OnHealthChangedEvent;
            TargetSystem.enemySelectedTarget.OnDeathEvent -= EnemySelectedTarget_OnDeathEvent;
        }

        #endregion

        #region PlayerTargetSelection

        private void TargetSystem_OnPlayerTargetSelectedEvent(object sender, EventArgs e)
        {
            //when we have a player target selected, enable the pov cam
            if (playerTargetUI) playerTargetUI.SetActive(true);

            if (playerTargetHealthUI)
            {
                playerTargetHealthUI.SetMaxValue(TargetSystem.playerSelectedTarget.GetMaxHealth());
                playerTargetHealthUI.SetCurrentValue(TargetSystem.playerSelectedTarget.GetCurrentHealth());
            }

            TargetSystem.playerSelectedTarget.OnHealthChangedEvent += PlayerSelectedTarget_OnHealthChangedEvent;
            TargetSystem.playerSelectedTarget.OnDeathEvent += PlayerSelectedTarget_OnDeathEvent;
        }

        private void TargetSystem_OnPlayerTargetDeSelectedEvent(object sender, EventArgs e)
        {
            //when we have a player target get deselected, disable
            if (playerTargetUI) playerTargetUI.SetActive(false);

            TargetSystem.playerSelectedTarget.OnHealthChangedEvent -= PlayerSelectedTarget_OnHealthChangedEvent;
            TargetSystem.playerSelectedTarget.OnDeathEvent -= PlayerSelectedTarget_OnDeathEvent;
        }

        private void PlayerSelectedTarget_OnHealthChangedEvent(object sender, EventArgs e)
        {
            if (playerTargetHealthUI)
            {
                playerTargetHealthUI.SetCurrentValue(TargetSystem.playerSelectedTarget.GetCurrentHealth());
            }
        }

        private void PlayerSelectedTarget_OnDeathEvent(object sender, Events.GameObjectEventArgs e)
        {
            //when we have a player target get killed, disable
            if (playerTargetUI) playerTargetUI.SetActive(false);

            TargetSystem.playerSelectedTarget.OnHealthChangedEvent -= PlayerSelectedTarget_OnHealthChangedEvent;
            TargetSystem.playerSelectedTarget.OnDeathEvent -= PlayerSelectedTarget_OnDeathEvent;
        }

        #endregion

        private void GameManager_OnLevelCompleteEvent(object sender, EventArgs e)
        {
            if (levelCompleteUI) levelCompleteUI.SetActive(true);
        }

        private void GameManager_OnLevelFailEvent(object sender, EventArgs e)
        {
            if (levelFailUI) levelFailUI.SetActive(true);
        }

        private void PauseManager_OnPausedChangedEvent(object sender, EventArgs e)
        {
            if (PauseManager.IsPaused)
            {
                if (gameUI) gameUI.SetActive(false);
                if (menuUI) menuUI.SetActive(false);

                if (pauseUI) pauseUI.SetActive(true);
            }

            else if (!PauseManager.IsPaused)
            {
                ResetGameUI();
            }
        }

        private void ResetGameUI()
        {
            if (optionsMenu) optionsMenu.SetActive(false);
            if (controlsPanel) controlsPanel.SetActive(false);
            if (videoPanel) videoPanel.SetActive(false);
            if (audioPanel) audioPanel.SetActive(false);

            if (gameUI) gameUI.SetActive(true);
            if (menuUI) menuUI.SetActive(true);

            if (pauseUI) pauseUI.SetActive(false);
        }

        public void ToggleFlir(bool b)
        {
            OnFlirToggled(b);
        }

        public void TogglePOVCam()
        {
            OnCameraToggled();
        }

        protected void RandomAltitudeUI()
        {
            if (altitudeTextUI) altitudeTextUI.text = "ALT: " +  NumberRandomiser.GetRandomInt(195, 205).ToString();
        }

        protected void RandomSpeedUI()
        {
            if (speedTextUI) speedTextUI.text = "SPD: " + NumberRandomiser.GetRandomInt(245, 255).ToString();
        }

        private void MoneyManager_OnMoneyChangedEvent(object sender, Events.IntEventArgs e)
        {
            if (moneyTextUI) moneyTextUI.text = "Cr: " + e.i;

            else
            {
                StopCoroutine(QueueUpdateMoneyText("Cr: " + e.i));
                StartCoroutine(QueueUpdateMoneyText("Cr: " + e.i));
            }

            //TODO: Lerp the money value??
        }

        private IEnumerator QueueUpdateMoneyText(string text)
        {
            while (!moneyTextUI.enabled)
            {
                yield return new WaitForEndOfFrame();
            }

            moneyTextUI.text = text;
        }

        private void Update()
        {
            if (timerTextUI) timerTextUI.text = LevelTimer.GetHoursAndMinutesAndSeconds();
            if (headingTextUI) headingTextUI.text = "HDG: " + ((int)Camera.main.transform.rotation.eulerAngles.y).ToString() + "°";
            if (verticalTextUI) verticalTextUI.text = "VRT: " + ((int)Camera.main.transform.rotation.eulerAngles.x).ToString() + "°";
        }

        //upgrade stuff

        public void ChassisUpgradeButton()
        {
            OnChassisUpgradedButton();
        }

        public void TurretUpgradeButton()
        {
            OnTurretUpgradedButton();
        }

        private void Shop_OnChassisUpgradedEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnChassisNotUpgradedEvent(object sender, EventArgs e)
        {
            //show ui saying we cannot repair this vehicle!
            gameUI.SetActive(false);
            chassisUpgradeUI.SetActive(false);
            chassisUpgradeFailUI.SetActive(true);
        }

        private void Shop_OnTurretUpgradedEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnTurretNotUpgradedEvent(object sender, EventArgs e)
        {
            //show ui saying we cannot repair this vehicle!
            gameUI.SetActive(false);
            turretUpgradeUI.SetActive(false);
            turretUpgradeFailUI.SetActive(true);
        }

        //vehicle purchasing stuff

        public void VehiclePurchaseButton()
        {
            OnVehiclePurchasedButton();
        }

        public void VehicleSellButton()
        {
            OnVehicleSoldButton();
        }

        private void Shop_OnVehiclePurchasedEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnVehicleNotPurchasedEvent(object sender, EventArgs e)
        {
            //show ui saying we cannot sell this vehicle!
            gameUI.SetActive(false);
            purchaseUI.SetActive(false);
            purchaseFailUI.SetActive(true);
        }

        private void Shop_OnVehicleSoldEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnVehicleNotSoldEvent(object sender, EventArgs e)
        {
            //show ui saying we cannot sell this vehicle!
            gameUI.SetActive(false);
            sellUI.SetActive(false);
            saleFailUI.SetActive(true);
        }

        //vehicle repair stuff

        public void VehicleRepairButton()
        {
            OnVehicleRepairedButton();
        }

        private void Shop_OnVehicleFullyRepairedEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnVehiclePartiallyRepairedEvent(object sender, EventArgs e)
        {
            gameUI.SetActive(true);
        }

        private void Shop_OnVehicleNotRepairedEvent(object sender, EventArgs e)
        {
            //show ui saying we cannot repair this vehicle!
            gameUI.SetActive(false);
            repairUI.SetActive(false);
            repairFailUI.SetActive(true);
        }
    }
}