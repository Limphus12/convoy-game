using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.limphus.settings;
using com.limphus.utilities;

namespace com.limphus.save_system
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem instance;

        private void Awake()
        {
            if (!instance) instance = this;
            else if (instance) Destroy(this);

            SaveManager.Init();

            PauseManager.OnPausedChangedEvent += PauseManager_OnPausedChangedEvent;
        }

        private void OnDestroy()
        {
            PauseManager.OnPausedChangedEvent -= PauseManager_OnPausedChangedEvent;
        }

        private void PauseManager_OnPausedChangedEvent(object sender, EventArgs e)
        {
            if (!PauseManager.IsPaused)
            {
                SaveCurrentSettings();
            }
        }

        //private void Update()
        //{
        //    Inputs();
        //}
        //
        //private void Inputs()
        //{
        //    if (Input.GetKeyDown(KeyCode.F5))
        //    {
        //        SettingsData data = new SettingsData
        //        {
        //            resolution = new ResolutionData { width = VideoSettings.resolution.width, height = VideoSettings.resolution.height, refreshRate = //VideoSettings.resolution.refreshRate },
        //            brightness = VideoSettings.brightness,
        //            isFullscreen = VideoSettings.isFullscreen,
        //            qualityIndex = VideoSettings.qualityIndex,
        //
        //            masterVolume = settings.AudioSettings.masterVolume,
        //            ambienceVolume = settings.AudioSettings.ambienceVolume,
        //            musicVolume = settings.AudioSettings.musicVolume,
        //            gameVolume = settings.AudioSettings.gameVolume,
        //            uiVolume = settings.AudioSettings.uiVolume,
        //        };
        //
        //        SaveSettings(data);
        //    }
        //
        //    if (Input.GetKeyDown(KeyCode.F9)) 
        //    {
        //        SettingsData data = LoadSettings();
        //
        //        //set the settings values
        //        VideoSettings.resolution = new Resolution { width = data.resolution.width, height = data.resolution.height, refreshRate = data.resolution.refreshRate };
        //        VideoSettings.brightness = data.brightness;
        //        VideoSettings.isFullscreen = data.isFullscreen;
        //        VideoSettings.qualityIndex = data.qualityIndex;
        //
        //        settings.AudioSettings.masterVolume = data.masterVolume;
        //        settings.AudioSettings.ambienceVolume = data.ambienceVolume;
        //        settings.AudioSettings.musicVolume = data.musicVolume;
        //        settings.AudioSettings.gameVolume = data.gameVolume;
        //        settings.AudioSettings.uiVolume = data.uiVolume;
        //    }
        //}

        public static void InitSettings()
        {
            //create a new settings save
            SettingsData data = new SettingsData
            {
                resolution = new ResolutionData { width = Screen.currentResolution.width, height = Screen.currentResolution.height, refreshRate = Screen.currentResolution.refreshRate },
                brightness = 1f, isFullscreen = Screen.fullScreen, qualityIndex = QualitySettings.GetQualityLevel(),

                masterVolume = 20f,
                ambienceVolume = -20f,
                gameVolume = -20f,
                uiVolume = -30f,
                musicVolume = 0f
            };

            //saving our new settings
            SaveSettings(data);
        }

        public static void LoadSettings()
        {
            //asking for the save string from teh save manager
            string saveString = SaveManager.Load(SaveManager.SETTINGS_SAVE_FILE);

            if (saveString != null)
            {
                //creating a save object from the json/string
                SettingsSaveObject saveObject = JsonUtility.FromJson<SettingsSaveObject>(saveString);

                SettingsData data = new SettingsData
                {
                    resolution = new ResolutionData { width = saveObject.resolution.width, height = saveObject.resolution.height, refreshRate = saveObject.resolution.refreshRate }, 
                    brightness = saveObject.brightness, isFullscreen = saveObject.isFullscreen, qualityIndex = saveObject.qualityIndex,

                    masterVolume = saveObject.masterVolume,
                    ambienceVolume = saveObject.ambienceVolume,
                    gameVolume = saveObject.gameVolume,
                    uiVolume = saveObject.uiVolume,
                    musicVolume = saveObject.musicVolume
                };
            }

            else
            {
                //create a new settings save
                SettingsData data = new SettingsData
                {
                    resolution = new ResolutionData { width = Screen.currentResolution.width, height = Screen.currentResolution.height, refreshRate = Screen.currentResolution.refreshRate },
                    brightness = 1f, isFullscreen = Screen.fullScreen, qualityIndex = QualitySettings.GetQualityLevel(),

                    masterVolume = 20f,
                    ambienceVolume = -20f,
                    gameVolume = -20f,
                    uiVolume = -30f,
                    musicVolume = 0f
                };

                SaveSettings(data);
            }
        }

        public void SaveCurrentSettings()
        {
            SettingsData data = new SettingsData
            {
                resolution = new ResolutionData { width = VideoSettings.resolution.width, height = VideoSettings.resolution.height, refreshRate = VideoSettings.resolution.refreshRate },
                brightness = VideoSettings.brightness,
                isFullscreen = VideoSettings.isFullscreen,
                qualityIndex = VideoSettings.qualityIndex,

                masterVolume = settings.AudioSettings.masterVolume,
                ambienceVolume = settings.AudioSettings.ambienceVolume,
                musicVolume = settings.AudioSettings.musicVolume,
                gameVolume = settings.AudioSettings.gameVolume,
                uiVolume = settings.AudioSettings.uiVolume,
            };

            SaveSettings(data);
        }

        public static void SaveSettings(SettingsData data)
        {
            //creating a new save object, settting the values
            SettingsSaveObject saveObject = new SettingsSaveObject
            {
                resolution = new Resolution { width = data.resolution.width, height = data.resolution.height, refreshRate = data.resolution.refreshRate }, 
                brightness = data.brightness, isFullscreen = data.isFullscreen, qualityIndex = data.qualityIndex,

                masterVolume = data.masterVolume, ambienceVolume = data.ambienceVolume,
                gameVolume = data.gameVolume, uiVolume = data.uiVolume,
                musicVolume = data.musicVolume
            };

            //using json utilities to write a json file
            string json = JsonUtility.ToJson(saveObject, true);

            //calling teh save method on the save manager
            SaveManager.Save(SaveManager.SETTINGS_SAVE_FILE, json);
        }

        private class SettingsSaveObject
        {
            //video
            public Resolution resolution; public float brightness; public bool isFullscreen; public int qualityIndex;

            //audio
            public float masterVolume, ambienceVolume, gameVolume, uiVolume, musicVolume;
        }

        public struct SettingsData
        {
            //video
            public ResolutionData resolution; public float brightness; public bool isFullscreen; public int qualityIndex;

            //audio
            public float masterVolume, ambienceVolume, gameVolume, uiVolume, musicVolume;
        }
    }
}