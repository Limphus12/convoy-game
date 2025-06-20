using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class TargetMoney : MonoBehaviour
    {
        [SerializeField] private int moneyAmountOnDeath;

        private Target target;

        private void Awake()
        {
            target = GetComponent<Target>();

            if (target) target.OnDeathEvent += Target_OnDeathEvent;
        }

        private void Target_OnDeathEvent(object sender, Events.GameObjectEventArgs e)
        {
            MoneyManager.AddMoney(moneyAmountOnDeath);
        }

        private void OnDestroy()
        {
            if (target) target.OnDeathEvent -= Target_OnDeathEvent;
        }
    }
}