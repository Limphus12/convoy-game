using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.limphus.utilities
{
    public class PauseManager : MonoBehaviour
    {
        public static bool IsPaused = false;

        public static event EventHandler<EventArgs> OnPausedChangedEvent;
        protected void OnPausedChanged() => OnPausedChangedEvent?.Invoke(this, EventArgs.Empty);

        private void Awake()
        {
            InputManager.OnEscapeKeyDownEvent += InputManager_OnEscapeKeyDownEvent;
            ASyncLoader.OnLoadingChanged += ASyncLoader_OnLoadingChanged;
        }

        private void ASyncLoader_OnLoadingChanged(object sender, Events.BoolEventArgs e)
        {
            IsPaused = e.i;
        }

        private void OnDestroy()
        {
            InputManager.OnEscapeKeyDownEvent -= InputManager_OnEscapeKeyDownEvent;
        }

        private void InputManager_OnEscapeKeyDownEvent(object sender, EventArgs e)
        {
            IsPaused = !IsPaused;

            if (IsPaused) Pause();
            else if (!IsPaused) UnPause();

            OnPausedChanged();
        }

        public void TogglePause(bool isPaused)
        {
            IsPaused = isPaused;

            if (IsPaused) Pause();
            else if (!IsPaused) UnPause();
            
            OnPausedChanged();
        }

        private void Pause()
        {
            IsPaused = true;

            List<IPauseable> pauseables = FindInterfaces.ListOf<IPauseable>();

            foreach (IPauseable pauseable in pauseables)
            {
                pauseable.Pause();
            }

            Time.timeScale = 0f;

            OnPausedChanged();
        }

        private void UnPause()
        {
            IsPaused = false;

            List<IPauseable> pauseables = FindInterfaces.ListOf<IPauseable>();

            foreach (IPauseable pauseable in pauseables)
            {
                pauseable.UnPause();
            }

            Time.timeScale = 1f;

            OnPausedChanged();
        }
    }
}