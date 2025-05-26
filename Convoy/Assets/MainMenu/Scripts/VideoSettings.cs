using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.limphus.save_system;

namespace com.limphus.settings
{
    public class VideoSettings : MonoBehaviour
    {
        private static VideoSettings instance;

        [Header("Brightness Settings")]
        [SerializeField] private Slider brightnessSlider; [SerializeField] private TMP_Text brightnessText;
        
        [Header("Resolution Settings")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private Resolution[] resolutions;
        public static int resolutionIndex = 0;

        [Header("Quality Settings")]
        [SerializeField] private TMP_Dropdown qualityDropdown;

        [Header("Fullscreen Settings")]
        [SerializeField] private Toggle fullscreenToggle;

        public static Resolution resolution;
        public static float brightness;
        public static bool isFullscreen;
        public static int qualityIndex;

        private void Awake()
        {
            if (!instance) instance = this;
            else if (instance) Destroy(this);

            SaveSystem.OnSettingsLoadedEvent += SaveSystem_OnSettingsLoadedEvent;
            SaveSystem.OnSettingsChangedEvent += SaveSystem_OnSettingsChangedEvent;
        }

        private void SaveSystem_OnSettingsLoadedEvent(object sender, SaveSystemEvents.OnSettingsChangedEventArgs e)
        {
            resolutionIndex = e.i.resolutionIndex;
            resolutions = Screen.resolutions; resolution = resolutions[resolutionIndex];

            brightness = e.i.brightness;
            isFullscreen = e.i.isFullscreen;
            qualityIndex = e.i.qualityIndex;

            ApplyAllSettings();
        }

        private void SaveSystem_OnSettingsChangedEvent(object sender, SaveSystemEvents.OnSettingsChangedEventArgs e)
        {
            resolutionIndex = e.i.resolutionIndex;
            resolutions = Screen.resolutions; resolution = resolutions[resolutionIndex];

            brightness = e.i.brightness;
            isFullscreen = e.i.isFullscreen;
            qualityIndex = e.i.qualityIndex;

            ApplyAllSettings();
        }

        private void OnDestroy()
        {
            SaveSystem.OnSettingsLoadedEvent -= SaveSystem_OnSettingsLoadedEvent;
            SaveSystem.OnSettingsChangedEvent -= SaveSystem_OnSettingsChangedEvent;
        }

        private void Start()
        {
            GetScreenResolutions();
            ApplyAllSettings();
        }

        public void InitUI()
        {
            if (brightnessText) brightnessText.text = brightness.ToString("0.00");
            if (brightnessSlider) brightnessSlider.value = brightness;

            if (resolutionDropdown) resolutionDropdown.value = resolutionIndex;

            if (qualityDropdown) qualityDropdown.value = qualityIndex;

            if (fullscreenToggle) fullscreenToggle.isOn = isFullscreen;
        }

        private void GetScreenResolutions()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            resolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " X " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";

                options.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    resolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int i)
        {
            resolutionIndex = i;
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen = isFullscreen);

            if (resolutionDropdown) resolutionDropdown.value = resolutionIndex;
        }

        public void SetFullscreen(bool b) 
        { 
            isFullscreen = b; 
            Screen.fullScreen = b;

            if (fullscreenToggle) fullscreenToggle.isOn = b;
        }

        public void SetBrightness(float f) 
        {
            brightness = f;
            Screen.brightness = f;

            if (brightnessText) brightnessText.text = f.ToString("0.00");
        }

        public void SetQualityLevel(int i)
        {
            qualityIndex = i;
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        private void ApplyAllSettings()
        {
            Resolution resolution = resolutions[resolutionIndex]; Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen = isFullscreen);
            Screen.brightness = brightness;
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    }
}