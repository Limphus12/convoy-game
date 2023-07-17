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
            Debug.Log("Trigger Enter!");

            if (other.CompareTag("Player")) 
            { 
                triggerEvent?.Invoke();
                triggered = true; 
            }
        }
    }
}