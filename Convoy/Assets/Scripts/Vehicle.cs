using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class Vehicle : MonoBehaviour
    {
        public ChassisManager ChassisManager { get; private set; }

        private void Awake()
        {
            Init();
        }

        private void Init()
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

        public void SetPath(PathCreator pathCreator)
        {
            GetComponent<PathFollower>().SetPath(pathCreator);
        }
    }
}