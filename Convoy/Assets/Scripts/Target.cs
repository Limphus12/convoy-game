using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public enum TargetType { Player, Enemy }

    public class Target : MonoBehaviour, IDamageable
    {
        [SerializeField] private TargetType type; [Space, SerializeField] private int health;

        private int currentHealth;

        public TargetType GetTargetType => type;

        private void Awake()
        {
            currentHealth = health;
        }

        public void Damage(int amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        public void Death()
        {
            //TODO: Animations and whatnot.

            gameObject.SetActive(false);
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}