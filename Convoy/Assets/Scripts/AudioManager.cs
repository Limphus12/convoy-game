using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    public class AudioManager : MonoBehaviour, IPauseable
    {
        [SerializeField] private AudioSource[] audioSources;

        public void Pause()
        {
            foreach (AudioSource source in audioSources)
            {
                source.Pause();
            }
        }

        public void UnPause()
        {
            foreach (AudioSource source in audioSources)
            {
                source.UnPause();
            }
        }
    }
}