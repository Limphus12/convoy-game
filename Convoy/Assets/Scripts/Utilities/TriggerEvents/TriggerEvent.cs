using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] protected string colliderTag;
        protected bool triggered = false;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(colliderTag) && !triggered) 
            {
                triggered = true;
            }
        }
    }
}