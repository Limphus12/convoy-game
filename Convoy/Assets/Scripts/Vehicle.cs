using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

namespace com.limphus.convoy
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 moveDir;

        public ChassisManager ChassisManager { get; private set; }

        private void Awake()
        {
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
            GetComponent<BezierWalkerWithSpeed>().spline = spline;
        }

        private void Update()
        {
            //Move();
        }

        private void Move()
        {
            transform.position += speed * Time.deltaTime * moveDir.normalized;
        }
    }
}