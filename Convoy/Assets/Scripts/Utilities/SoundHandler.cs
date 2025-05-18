using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace com.limphus.utilities
{
    [System.Serializable]
    public struct SoundDataStruct
    {
        public AudioMixerGroup audioMixerGroup;
        public AudioClip[] clips;
        public float volume, pitch, spatialBlend, minDistance, maxDistance;
        public AudioRolloffMode rolloffMode;

        public bool randomisePitch;
        public float minPitch, maxPitch;
    }

    public static class SoundHandler
    {
        private static float RandomisePitch(float min, float max) => Random.Range(min, max);

        public static void PlaySound(SoundDataStruct soundDataStruct, Transform parent, Vector3 position)
        {
            if (soundDataStruct.clips.Length == 0)
            {
                Debug.LogWarning("No Audio Clips found! Assign Audio Clips!");
                return;
            }

            GameObject audioGameObject = new GameObject("AudioSource");
            audioGameObject.transform.parent = parent;
            audioGameObject.transform.position = position;

            AudioSource audioSource = audioGameObject.AddComponent<AudioSource>();

            int i = 0;

            //Single Clip
            if (soundDataStruct.clips.Length == 1)
            {
                audioSource.clip = soundDataStruct.clips[i];
            }

            //Random Clip
            else if (soundDataStruct.clips.Length > 1)
            {
                i = Random.Range(0, soundDataStruct.clips.Length - 1);

                audioSource.clip = soundDataStruct.clips[i];
            }

            audioSource.outputAudioMixerGroup = soundDataStruct.audioMixerGroup;
            audioSource.spatialBlend = soundDataStruct.spatialBlend;
            audioSource.volume = soundDataStruct.volume;

            if (soundDataStruct.randomisePitch)
            {
                audioSource.pitch = RandomisePitch(soundDataStruct.minPitch, soundDataStruct.maxPitch);
            }

            else audioSource.pitch = soundDataStruct.pitch;

            audioSource.rolloffMode = soundDataStruct.rolloffMode;

            audioSource.minDistance = soundDataStruct.minDistance;
            audioSource.maxDistance = soundDataStruct.maxDistance;

            audioSource.Play();

            Object.Destroy(audioGameObject, soundDataStruct.clips[i].length);
        }
    }
}