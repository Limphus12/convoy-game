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

        }

        private void Update()
        {
            CheckTargets();
        }

        private void CheckTargets()
        {

        }

        public void OnLevelStart()
        {

        }

        public void OnLevelEnd()
        {
            Debug.Log("Finished the Level!");
        }

        public void OnLevelFail()
        {

        }
    }
}