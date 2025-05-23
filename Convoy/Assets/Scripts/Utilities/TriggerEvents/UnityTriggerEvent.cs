using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class UnityTriggerEvent : TriggerEvent
    {
        [SerializeField] protected UnityEvent triggerEvent;

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(colliderTag) && !triggered)
            {
                triggerEvent?.Invoke();
                triggered = true;
            }
        }
    }
}