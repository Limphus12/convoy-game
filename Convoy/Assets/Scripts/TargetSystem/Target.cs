using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class Target : MonoBehaviour, IDamageable
    {
        [SerializeField] private TargetType type; [Space, SerializeField] private int health;

        private int currentHealth;

        [SerializeField] private Slider healthBar;

        public TargetType GetTargetType => type;

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
            //TODO: Animations and whatnot.
            //probably just spawn a prefab?

            Destroy(gameObject);
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}