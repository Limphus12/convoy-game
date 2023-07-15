using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class TargetSystem : MonoBehaviour
    {
        [SerializeField] private float updateInterval;

        public static Target[] targetArray;

        public static Target playerSelectedTarget;

        private void Start() => InvokeRepeating(nameof(UpdateTargets), 0f, updateInterval);
        
        public static bool HasTargets() => targetArray.Length > 0;

        private void UpdateTargets()
        {
            targetArray = FindObjectsOfType<Target>();
        }

        // Update is called once per frame
        void Update()
        {
            Inputs();
        }

        private void Inputs()
        {
            if (Input.GetMouseButtonDown(2))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    Target target = hit.transform.GetComponent<Target>();

                    if (target != null)
                    {
                        if (!target.IsDead()) playerSelectedTarget = target;
                        else playerSelectedTarget = null;
                    }
                }
            }
        }
    }
}