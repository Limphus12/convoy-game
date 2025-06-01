using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class Vehicle : MonoBehaviour
    {
        public ChassisManager ChassisManager { get; private set;}
        public TurretManager TurretManager { get; private set; }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            ChassisManager = GetComponent<ChassisManager>();
            TurretManager = GetComponent<TurretManager>();
        }

        public void SetPath(PathCreator pathCreator)
        {
            GetComponent<PathFollower>().SetPath(pathCreator);
        }
    }
}