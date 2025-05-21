using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] protected UnityEvent triggerEvent;
        [SerializeField] protected string colliderTag;

        protected bool triggered = false;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(colliderTag) && !triggered) 
            { 
                triggerEvent?.Invoke();
                triggered = true;
            }
        }
    }
}