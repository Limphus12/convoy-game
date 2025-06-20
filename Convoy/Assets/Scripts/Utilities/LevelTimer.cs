using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.limphus.utilities
{
    //code source - https://discussions.unity.com/t/timer-with-string-format/880462

    public class LevelTimer : MonoBehaviour
    {
        static float time = 0f;

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            time = 0f;
        }

        public static string GetHoursAndMinutesAndSeconds()
        {
            int seconds = (int)time % 60; int minutes = (int)time / 60; int hours = (int)time / 600;
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        public static string GetMinutesAndSecondsText()
        {
            int seconds = (int)time % 60; int minutes = (int)time / 60;
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            time += Time.deltaTime;
        }
    }
}