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
        //TODO: Level Tracking, Prestige Tracking etc. (like colossatron)




        private void Awake()
        {
            TargetSystem.OnPlayerTargetsEmptyEvent += TargetSystem_OnPlayerTargetsEmptyEvent;

            LevelStart();
        }

        private void OnDestroy()
        {
            TargetSystem.OnVisiblePlayerTargetsEmptyEvent -= TargetSystem_OnPlayerTargetsEmptyEvent;
        }

        private void TargetSystem_OnPlayerTargetsEmptyEvent(object sender, EventArgs e)
        {
            Debug.Log("No Visible Player Targets!");

            LevelFail();
        }

        public void LevelStart()
        {
            Debug.Log("Started the Level!");
        }

        public void LevelEnd()
        {
            Debug.Log("Finished the Level!");
        }

        public void LevelFail()
        {
            Debug.Log("Failed the Level!");
        }
    }
}