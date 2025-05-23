using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

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

        public void SetMasterVolume(float i)
        {
            masterVolume = i;

            if (masterMixer) masterMixer.SetFloat("MasterVolume", masterVolume);

            if (masterVolumeText) masterVolumeText.text = masterVolume.ToString();
        }

        public void SetMusicVolume(float i)
        {
            musicVolume = i;

            if (masterMixer) masterMixer.SetFloat("MusicVolume", musicVolume);

            if (musicVolumeText) musicVolumeText.text = musicVolume.ToString();
        }

        public void SetAmbienceVolume(float i)
        {
            ambienceVolume = i;

            if (masterMixer) masterMixer.SetFloat("AmbienceVolume", ambienceVolume);

            if (ambienceVolumeText) ambienceVolumeText.text = ambienceVolume.ToString();
        }

        public void SetGameVolume(float i)
        {
            gameVolume = i;

            if (masterMixer) masterMixer.SetFloat("GameVolume", gameVolume);

            if (gameVolumeText) gameVolumeText.text = gameVolume.ToString();
        }

        public void SetUIVolume(float i)
        {
            uiVolume = i;

            if (masterMixer) masterMixer.SetFloat("UIVolume", uiVolume);

            if (uiVolumeText) uiVolumeText.text = uiVolume.ToString();
        }
    }
}