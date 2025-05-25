using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

namespace com.limphus.settings
{
    public class AudioSettings : MonoBehaviour
    {
        private static AudioSettings instance;

        [Header("Audio Settings")]
        [SerializeField] private AudioMixer masterMixer;

        [Space, SerializeField] private Slider masterVolumeSlider; [SerializeField] private TMP_Text masterVolumeText; 
        [Space, SerializeField] private Slider ambienceVolumeSlider; [SerializeField] private TMP_Text ambienceVolumeText;
        [Space, SerializeField] private Slider musicVolumeSlider;[SerializeField] private TMP_Text musicVolumeText;
        [Space, SerializeField] private Slider gameVolumeSlider; [SerializeField] private TMP_Text gameVolumeText;
        [Space, SerializeField] private Slider uiVolumeSlider;[SerializeField] private TMP_Text uiVolumeText;

        public static float masterVolume, ambienceVolume, musicVolume, gameVolume, uiVolume;

        //TODO: look at UnityEngine.AudioSettings; that seems to have some cool audio stuff, like speaker settings and whatnot

        private void Awake()
        {
            if (!instance) instance = this;
            else if (instance) Destroy(this);
        }

        public void InitUI()
        {
            if (masterVolumeText) masterVolumeText.text = (masterVolume + 80).ToString();
            if (musicVolumeText) musicVolumeText.text = (musicVolume + 80).ToString();
            if (ambienceVolumeText) ambienceVolumeText.text = (ambienceVolume + 80).ToString();
            if (gameVolumeText) gameVolumeText.text = (gameVolume + 80).ToString();
            if (uiVolumeText) uiVolumeText.text = (uiVolume + 80).ToString();

            if (masterVolumeSlider) masterVolumeSlider.value = masterVolume;
            if (musicVolumeSlider) musicVolumeSlider.value = musicVolume;
            if (ambienceVolumeSlider) ambienceVolumeSlider.value = ambienceVolume;
            if (gameVolumeSlider) gameVolumeSlider.value = gameVolume;
            if (uiVolumeSlider) uiVolumeSlider.value = uiVolume;
        }

        public void SetMasterVolume(float i)
        {
            masterVolume = i;

            if (masterMixer) masterMixer.SetFloat("MasterVolume", masterVolume);

            if (masterVolumeText) masterVolumeText.text = (masterVolume + 80).ToString();
        }

        public void SetMusicVolume(float i)
        {
            musicVolume = i;

            if (masterMixer) masterMixer.SetFloat("MusicVolume", musicVolume);

            if (musicVolumeText) musicVolumeText.text = (musicVolume + 80).ToString();
        }

        public void SetAmbienceVolume(float i)
        {
            ambienceVolume = i;

            if (masterMixer) masterMixer.SetFloat("AmbienceVolume", ambienceVolume);

            if (ambienceVolumeText) ambienceVolumeText.text = (ambienceVolume + 80).ToString();
        }

        public void SetGameVolume(float i)
        {
            gameVolume = i;

            if (masterMixer) masterMixer.SetFloat("SFXVolume", gameVolume);

            if (gameVolumeText) gameVolumeText.text = (gameVolume + 80).ToString();
        }

        public void SetUIVolume(float i)
        {
            uiVolume = i;

            if (masterMixer) masterMixer.SetFloat("UIVolume", uiVolume);

            if (uiVolumeText) uiVolumeText.text = (uiVolume + 80).ToString();
        }
    }
}