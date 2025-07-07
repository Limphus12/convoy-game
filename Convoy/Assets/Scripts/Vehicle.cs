using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

namespace com.limphus.convoy
{
    public class Vehicle : MonoBehaviour
    {
        public ChassisManager ChassisManager { get; private set; }

        public Target Target { get; private set; }


        public bool isFirstVehicle = false;

        public bool canMove = true;

        private BezierWalkerWithSpeed walker;

        public BezierWalkerWithSpeed GetBezier() => walker;

        private void Awake()
        {
            walker = GetComponent<BezierWalkerWithSpeed>();

            Init();
        }

        private void Init()
        {
            PartManager.OnPartChangedEvent += PartManager_OnPartChangedEvent;

            FindChassisManager();
        }

        private void OnDestroy()
        {
            PartManager.OnPartChangedEvent -= PartManager_OnPartChangedEvent;
        }

        private void PartManager_OnPartChangedEvent(object sender, EventArgs e)
        {
            FindChassisManager();
            FindTarget();
        }

        private void FindTarget()
        {
            Target = GetComponentInChildren<Target>(true);
        }

        public Target GetTarget()
        {
            FindTarget();

            if (Target) return Target;

            else return null;
        }

        private void FindChassisManager()
        {
            ChassisManager = GetComponent<ChassisManager>();
        }

        public ChassisManager GetChassisManager()
        {
            FindChassisManager();

            if (ChassisManager) return ChassisManager;

            else return null;
        }

        public void SetBeizer(BezierSpline spline)
        {
            walker.spline = spline;
        }

        private void Update()
        {
            if (!isFirstVehicle && canMove) { Move(); Rotate(); }
        }

        private void Move()
        {
            int index = ConvoyManager.vehiclesList.IndexOf(this) - 1;

            Vector3 targetDir = (ConvoyManager.vehiclesList[index].transform.position - transform.position).normalized;

            Vector3 targetPos = ConvoyManager.vehiclesList[index].transform.position - (targetDir * ConvoyManager.currentVehicleSeperation);

            Vector3 lerpTargetPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * walker.speed);

            transform.position = lerpTargetPos;
        }

        private void Rotate()
        {
            int index = ConvoyManager.vehiclesList.IndexOf(this) - 1;

            transform.LookAt(ConvoyManager.vehiclesList[index].transform);
        }
    }
}