using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public enum TargetType { Player, Enemy }

    public class TargetSystem : MonoBehaviour
    {
        [SerializeField] private float updateInterval;

        public static List<Target> playerTargets = new List<Target>();
        public static List<Target> enemyTargets = new List<Target>();

        public static Target playerSelectedTarget;

        private void Start() => InvokeRepeating(nameof(UpdateTargets), 0f, updateInterval);
        
        public static bool HasTargets() => enemyTargets.Count > 0;

        private void UpdateTargets()
        {
            Target[] targetArray = FindObjectsOfType<Target>();

            foreach(Target target in targetArray)
            {
                if (target.GetTargetType == TargetType.Player) playerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy) enemyTargets.Add(target);
            }
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
                        if (!target.IsDead() && target.GetTargetType == TargetType.Enemy) playerSelectedTarget = target;
                        else playerSelectedTarget = null;
                    }
                }
            }
        }
    }
}