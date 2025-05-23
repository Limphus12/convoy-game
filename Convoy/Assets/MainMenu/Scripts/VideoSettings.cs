using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.limphus.settings
{
    [Serializable]
    public struct ResolutionData
    {
        //
        // Summary:
        //     Resolution width in pixels.
        public int width { get; set; }
        //
        // Summary:
        //     Resolution height in pixels.
        public int height { get; set; }
        //
        // Summary:
        //     Resolution's vertical refresh rate in Hz.
        public int refreshRate { get; set; }
    }

    public class VideoSettings : MonoBehaviour
    {
        private static VideoSettings instance;

        [Header("Brightness Settings")]
        [SerializeField] private Slider brightnessSlider; [SerializeField] private TMP_Text brightnessText;
        
        [Header("Resolution Settings")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private Resolution[] resolutions;

        public static Resolution resolution;
        public static float brightness;
        public static bool isFullscreen;
        public static int qualityIndex;

        private void Awake()
        {
            if (!instance) instance = this;
            else if (instance) Destroy(this);

            Init();
        }

        private void Init()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " X " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";

                options.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int i)
        {
            Resolution resolution = resolutions[i];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullscreen(bool b) { isFullscreen = b; Screen.fullScreen = b; }

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
    }
}