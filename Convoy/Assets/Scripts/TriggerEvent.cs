using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.convoy
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggerEvent;
        [SerializeField] private string colliderTag;

        bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(colliderTag)) 
            { 
                triggerEvent?.Invoke();
                triggered = true; 
            }
        }
    }
}