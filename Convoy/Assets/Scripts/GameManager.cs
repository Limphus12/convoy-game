using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public enum GameState { Start, Complete, Fail }

    public class GameManager : MonoBehaviour
    {
        //TODO: Level Tracking, Prestige Tracking etc. (like colossatron)

        public static event EventHandler<EventArgs> OnLevelStartEvent, OnLevelCompleteEvent, OnLevelFailEvent;

        protected static void OnLevelStart() => OnLevelStartEvent?.Invoke(typeof(GameManager), EventArgs.Empty);
        protected static void OnLevelComplete() => OnLevelCompleteEvent?.Invoke(typeof(GameManager), EventArgs.Empty);
        protected static void OnLevelFail() => OnLevelFailEvent?.Invoke(typeof(GameManager), EventArgs.Empty);


        private void Awake()
        {
            TargetSystem.OnPlayerTargetsEmptyEvent += TargetSystem_OnPlayerTargetsEmptyEvent;

            ChangeGameState(GameState.Start);
        }

        private void OnDestroy()
        {
            TargetSystem.OnVisiblePlayerTargetsEmptyEvent -= TargetSystem_OnPlayerTargetsEmptyEvent;
        }

        private void TargetSystem_OnPlayerTargetsEmptyEvent(object sender, EventArgs e)
        {
            ChangeGameState(GameState.Fail);
        }

        public static GameState currentGameState;

        public static void ChangeGameState(GameState newGameState)
        {
            if (newGameState == currentGameState) return;

            switch (newGameState)
            {
                case GameState.Start:

                    OnLevelStart();

                    break;
                case GameState.Complete:

                    OnLevelComplete();

                    break;
                case GameState.Fail:

                    OnLevelFail();

                    break;

                default:
                    break;
            }
        }
    }
}