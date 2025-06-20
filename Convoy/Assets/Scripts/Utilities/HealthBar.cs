using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.utilities
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private RawImage fill;

        private int currentValue, maxValue;

        public void SetMaxValue(int value)
        {
            if (!healthBar) return;

            maxValue = value;

            healthBar.maxValue = maxValue;
        }

        public void SetCurrentValue(int value)
        {
            if (!healthBar) return;

            currentValue = value;

            healthBar.value = currentValue;
        }

        public void SetColor(Color color)
        {
            if (!fill) return;

            fill.color = color;
        }
    }
}