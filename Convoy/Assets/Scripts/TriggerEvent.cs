using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.convoy
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnterEvent;
        [SerializeField] private string colliderTag;

        bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger Enter!");

            if (other.CompareTag("Player")) 
            { 
                onTriggerEnterEvent?.Invoke();
                triggered = true; 
            }
        }
    }
}