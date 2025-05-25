using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public enum TargetType { Player, Enemy }

    public class TargetSystem : MonoBehaviour, IPauseable
    {
        #region Variables

        [SerializeField] private float updateInterval;

        public static List<Target> playerTargets = new List<Target>();
        public static List<Target> enemyTargets = new List<Target>();

        public static List<Target> visiblePlayerTargets = new List<Target>();
        public static List<Target> visibleEnemyTargets = new List<Target>();

        public static Target playerSelectedTarget;

        private Camera cam;

        public bool IsPaused { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        #region Events

        public static event EventHandler<EventArgs> OnPlayerTargetsUpdatedEvent, OnPlayerTargetsEmptyEvent, OnVisiblePlayerTargetsUpdatedEvent, OnVisiblePlayerTargetsEmptyEvent;
        public static event EventHandler<EventArgs> OnEnemyTargetsUpdatedEvent, OnEnemyTargetsEmptyEvent, OnVisibleEnemyTargetsUpdatedEvent, OnVisibleEnemyTargetsEmptyEvent;

        protected void OnPlayerTargetsUpdated() => OnPlayerTargetsUpdatedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnPlayerTargetsEmpty() => OnPlayerTargetsEmptyEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVisiblePlayerTargetsUpdated() => OnVisiblePlayerTargetsUpdatedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVisiblePlayerTargetsEmpty() => OnVisiblePlayerTargetsEmptyEvent?.Invoke(this, EventArgs.Empty);

        protected void OnEnemyTargetsUpdated() => OnEnemyTargetsUpdatedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnEnemyTargetsEmpty() => OnEnemyTargetsEmptyEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVisibleEnemyTargetsUpdated() => OnVisibleEnemyTargetsUpdatedEvent?.Invoke(this, EventArgs.Empty);
        protected void OnVisibleEnemyTargetsEmpty() => OnVisibleEnemyTargetsEmptyEvent?.Invoke(this, EventArgs.Empty);

        #endregion

        private void Awake()
        {
            if (!cam) cam = Camera.main;

            InputManager.OnMiddleMouseDownEvent += InputManager_OnMiddleMouseDownEvent;
        }

        private void OnDestroy()
        {
            InputManager.OnMiddleMouseDownEvent -= InputManager_OnMiddleMouseDownEvent;
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
            //make sure to cull the lists of dead/null targets!
            RemoveNullTargets();

            Target[] targetArray = FindObjectsOfType<Target>();

            foreach(Target target in targetArray)
            {
                if (target == null) return;

                //firstly add the target to the appropriate list (making sure we're not adding them multiple times)
                if (target.GetTargetType == TargetType.Player && !playerTargets.Contains(target)) playerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy && !enemyTargets.Contains(target)) enemyTargets.Add(target);
                
                if (!TargetVisible(target.gameObject)) continue; //check if the target is on screen

                //then add the visible targets to a seperate list (making sure we're not adding them multiple times)
                if (target.GetTargetType == TargetType.Player && !visiblePlayerTargets.Contains(target)) visiblePlayerTargets.Add(target);
                else if (target.GetTargetType == TargetType.Enemy && !visibleEnemyTargets.Contains(target)) visibleEnemyTargets.Add(target);
            }

            CheckTargets();
        }

        private void CheckTargets()
        {
            if (playerTargets.Count == 0) OnPlayerTargetsEmpty();
            if (enemyTargets.Count == 0) OnEnemyTargetsEmpty();
            if (visiblePlayerTargets.Count == 0) OnVisiblePlayerTargetsEmpty();
            if (visibleEnemyTargets.Count == 0) OnVisibleEnemyTargetsEmpty();
        }

        private void RemoveNullTargets()
        {
            for (int i = 0; i < playerTargets.Count; i++)
            {
                if (playerTargets[i] == null) playerTargets.Remove(playerTargets[i]);
            }

            for (int i = 0; i < visiblePlayerTargets.Count; i++)
            {
                if (visiblePlayerTargets[i] == null) visiblePlayerTargets.Remove(visiblePlayerTargets[i]);
            }

            for (int i = 0; i < enemyTargets.Count; i++)
            {
                if (enemyTargets[i] == null) enemyTargets.Remove(enemyTargets[i]);
            }

            for (int i = 0; i < visibleEnemyTargets.Count; i++)
            {
                if (visibleEnemyTargets[i] == null) visibleEnemyTargets.Remove(visibleEnemyTargets[i]);
            }
        }

        public void Pause()
        {
            InputManager.OnMiddleMouseDownEvent -= InputManager_OnMiddleMouseDownEvent;

            CancelInvoke(nameof(UpdateTargets));
        }

        public void UnPause()
        {
            InputManager.OnMiddleMouseDownEvent += InputManager_OnMiddleMouseDownEvent;

            InvokeRepeating(nameof(UpdateTargets), 0f, updateInterval);
        }
    }
}