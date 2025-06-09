using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class TargetVFX : MonoBehaviour
    {
        [SerializeField] private GameObject deathPrefab;

        private Target target;

        private void Awake()
        {
            target = GetComponent<Target>();

            if (target) target.OnDeathEvent += Target_OnDeathEvent;
        }

        private void Target_OnDeathEvent(object sender, utilities.Events.GameObjectEventArgs e)
        {
            VFX();
        }

        private void VFX()
        {
            if (deathPrefab) Instantiate(deathPrefab, transform.position, transform.rotation);
        }
    }
}