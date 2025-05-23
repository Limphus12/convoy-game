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

        private void ASyncLoader_OnLoadingChanged(object sender, Events.OnBoolChangedEventArgs e)
        {
            IsPaused = e.i;
        }

        private void OnDestroy()
        {
            InputManager.OnEscapeKeyDownEvent -= InputManager_OnEscapeKeyDownEvent;
        }

        private void InputManager_OnEscapeKeyDownEvent(object sender, System.EventArgs e)
        {
            IsPaused = !IsPaused;

            List<IPauseable> pauseables = FindInterfaces.ListOf<IPauseable>();

            foreach (IPauseable pauseable in pauseables)
            {
                if (IsPaused) pauseable.Pause();
                else if (!IsPaused) pauseable.Unpause();
            }

            OnPausedChanged();
        }
    }
}