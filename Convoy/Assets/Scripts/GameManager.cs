using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public enum GameState { }

    public class GameManager : MonoBehaviour
    {

        private void Awake()
        {
            OnLevelStart();
        }

        private void Update()
        {
            CheckPlayerTargets();
        }

        private void CheckPlayerTargets()
        {
            if (TargetSystem.visiblePlayerTargets.Count == 0)
            {
                Debug.Log("No Player Targets!");
            }
        }

        public void OnLevelStart()
        {
            Debug.Log("Started the Level!");
        }

        public void OnLevelEnd()
        {
            Debug.Log("Finished the Level!");
        }

        public void OnLevelFail()
        {
            Debug.Log("Failed the Level!");
        }
    }
}