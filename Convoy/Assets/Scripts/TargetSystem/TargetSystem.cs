using System;
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

        public static List<Target> visiblePlayerTargets = new List<Target>();
        public static List<Target> visibleEnemyTargets = new List<Target>();

        public static Target playerSelectedTarget;

        private Camera cam;

        private void Awake()
        {
            if (!cam) cam = Camera.main;

            InputManager.OnMiddleMouseDownEvent += InputManager_OnMiddleMouseDownEvent;
        }

        private void Start() => InvokeRepeating(nameof(UpdateTargets), 0f, updateInterval);

        private void InputManager_OnMiddleMouseDownEvent(object sender, EventArgs e)
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

            else playerSelectedTarget = null;
        }

        public static bool HasTargets() => visibleEnemyTargets.Count > 0;

        private bool TargetVisible(GameObject target)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);

            var point = target.transform.position;

            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0) return false;
            }

            return true;
        }

        private void UpdateTargets()
        {
            Target[] targetArray = FindObjectsOfType<Target>();

            foreach(Target target in targetArray)
            {
                //firstly add the target to the appropriate list
                if (target.GetTargetType == TargetType.Player) playerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy) enemyTargets.Add(target);

                if (!TargetVisible(target.gameObject)) continue; //check if the target is on screen

                //then add the visible targets to a seperate list
                if (target.GetTargetType == TargetType.Player) visiblePlayerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy) visibleEnemyTargets.Add(target);
            }
        }
    }
}