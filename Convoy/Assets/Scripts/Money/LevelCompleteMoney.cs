using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class LevelCompleteMoney : MonoBehaviour
    {
        [SerializeField] private int moneyAmountOnLevelComplete;

        private void Awake()
        {
            GameManager.OnLevelCompleteEvent += GameManager_OnLevelCompleteEvent;
        }

        private void GameManager_OnLevelCompleteEvent(object sender, EventArgs e)
        {
            MoneyManager.AddMoney(moneyAmountOnLevelComplete);
        }

        private void OnDestroy()
        {
            GameManager.OnLevelCompleteEvent -= GameManager_OnLevelCompleteEvent;
        }
    }

}
