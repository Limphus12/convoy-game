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

        [SerializeField] private WorldFollow selectedTargetUI;

        [SerializeField] private GameObject targetCanvas;
        [SerializeField] private WorldFollow playerTargetUI, enemyTargetUI;

        public static List<Target> playerTargets = new List<Target>();
        public static List<Target> enemyTargets = new List<Target>();

        public static Target playerSelectedTarget;

        private Camera cam;

        private void Awake()
        {
            if (!cam) cam = Camera.main;
        }

        private void Start() => InvokeRepeating(nameof(UpdateTargets), 0f, updateInterval);
        
        public static bool HasTargets() => enemyTargets.Count > 0;

        private bool TargetVisible(GameObject target)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);

            var point = target.transform.position;

            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0) return false;
            }

            //additional raycast check to ensure the target is actually visible
            //if (Physics.Raycast(transform.position, point - transform.position, out RaycastHit hit, Mathf.Infinity))
            //{
            //    return hit.collider.gameObject == target;
            //}

            //else return false;

            return true;
        }

        private void UpdateTargets()
        {
            Target[] targetArray = FindObjectsOfType<Target>();

            foreach(Target target in targetArray)
            {
                if (!TargetVisible(target.gameObject)) continue; //check if the target is on screen

                if (target.GetTargetType == TargetType.Player) playerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy) enemyTargets.Add(target);
            }
        }

        //TODO: Move this to a seperate UI class

        private void TargetUI()
        {
            if (targetCanvas)
            {
                if (enemyTargetUI)
                {
                    int i = 0;

                    foreach (Target target in enemyTargets)
                    {
                        //if the target already has a ui thingy, just move onto the next one.
                        if (target == null) continue;

                        WorldFollow worldFollow = target.GetComponentInChildren<WorldFollow>();

                        if (worldFollow != null)
                        {
                            //destroys the original target canvas if we have selected (no overlapping ui).
                            if (target == playerSelectedTarget)
                            {
                                Canvas canvas1 = worldFollow.GetComponentInParent<Canvas>();

                                if (canvas1 != null) Destroy(canvas1.gameObject);
                            }

                            continue;
                        }

                        GameObject canvas = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(enemyTargetUI, canvas.transform);

                        ui.SetTarget(target.transform);

                        i++;
                    }
                }

                if (playerTargetUI)
                {
                    int i = 0;

                    foreach (Target target in playerTargets)
                    {
                        //if the target already has a ui thingy, just move onto the next one.
                        if (target == null) continue;
                        if (target.GetComponentInChildren<WorldFollow>()) continue;

                        GameObject canvas = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(playerTargetUI, canvas.transform);

                        ui.SetTarget(target.transform);

                        i++;
                    }
                }
            }
        }

        void Update()
        {
            Inputs(); UI(); TargetUI();
        }

        //TODO: Move this to a seperate UI class
        private void UI()
        {
            if (!playerSelectedTarget && selectedTargetUI)
            {
                selectedTargetUI.gameObject.SetActive(false);
            }

            else if (playerSelectedTarget && selectedTargetUI)
            {
                selectedTargetUI.gameObject.SetActive(true);

                selectedTargetUI.SetTarget(playerSelectedTarget.transform);
            }
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

                else playerSelectedTarget = null;
            }
        }
    }
}