using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class Target : MonoBehaviour, IDamageable
    {
        [SerializeField] private TargetType type; [Space, SerializeField] private int health;

        private int currentHealth;

        [SerializeField] private Slider healthBar;

        public TargetType GetTargetType => type;

        public event EventHandler<Events.GameObjectEventArgs> OnDeathEvent;
        protected void OnDeath() => OnDeathEvent?.Invoke(this, new Events.GameObjectEventArgs { i = gameObject });

        private void Awake()
        {
            currentHealth = health;

            if (healthBar)
            {
                healthBar.maxValue = health;
                healthBar.value = currentHealth;
            }
        }

        public int GetMaxHealth() => health;
        public int GetCurrentHealth() => currentHealth;

        public void Damage(int amount)
        {
            currentHealth -= amount;

            if (healthBar)
            {
                healthBar.value = currentHealth;
            }

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        public void Death()
        {
            OnDeath();

            Destroy(transform.parent.gameObject);
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}