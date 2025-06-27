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
        [SerializeField] private TargetType type; [Space, SerializeField] private int maxHealth;

        private int currentHealth;

        [SerializeField] private HealthBar healthBar;

        public TargetType GetTargetType => type;

        public event EventHandler<Events.GameObjectEventArgs> OnDeathEvent;
        public event EventHandler<EventArgs> OnHealthChangedEvent;
        protected void OnDeath() => OnDeathEvent?.Invoke(this, new Events.GameObjectEventArgs { i = gameObject });
        protected void OnHealthChanged() => OnHealthChangedEvent?.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            currentHealth = maxHealth;

            if (healthBar)
            {
                healthBar.SetMaxValue(maxHealth);
                healthBar.SetCurrentValue(currentHealth);
            }
        }

        public int GetMaxHealth() => maxHealth;
        public int GetCurrentHealth() => currentHealth;

        public void SetMaxHealth(int health) => maxHealth = health;
        public void SetCurrentHealth(int health) => currentHealth = health;

        public void Heal(int amount)
        {
            currentHealth += amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnHealthChanged();

            if (healthBar)
            {
                healthBar.SetCurrentValue(currentHealth);
            }
        }

        public void Damage(int amount)
        {
            currentHealth -= amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnHealthChanged();

            if (healthBar)
            {
                healthBar.SetCurrentValue(currentHealth);
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