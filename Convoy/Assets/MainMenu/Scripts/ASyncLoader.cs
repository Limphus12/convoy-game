using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.limphus.utilities
{
    public class ASyncLoader : MonoBehaviour
    {
        public static bool IsLoadingScene;

        public static event EventHandler<EventArgs> OnLoadLevelBtnPressed;
        public static event EventHandler<Events.OnBoolChangedEventArgs> OnLoadingChanged;
        public static event EventHandler<Events.OnFloatChangedEventArgs> OnLoadProgressValueChanged;

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) => ResetLoadingScene();

        public void Quit()
        {
            Application.Quit();
        }

        public void LoadLevelBtn(string levelToLoad)
        {
            if (IsLoadingScene) return;

            OnLoadLevelBtnPressed?.Invoke(this, EventArgs.Empty);

            StartCoroutine(LoadLevelAsync(levelToLoad));
        }

        public void LoadLevelBtn(int levelToLoad)
        {
            if (IsLoadingScene) return;

            OnLoadLevelBtnPressed?.Invoke(this, EventArgs.Empty);

            StartCoroutine(LoadLevelAsync(levelToLoad));
        }

        private IEnumerator LoadLevelAsync(string levelToLoad)
        {
            StartLoadingScene();

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (!loadOperation.isDone)
            {
                AsyncLoadScene(loadOperation);

                yield return null;
            }
        }

        private IEnumerator LoadLevelAsync(int levelToLoad)
        {
            StartLoadingScene();

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (!loadOperation.isDone)
            {
                AsyncLoadScene(loadOperation);

                yield return null;
            }
        }

        private void AsyncLoadScene(AsyncOperation loadOperation)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            //use this to update ui and whatnot
            OnLoadProgressValueChanged?.Invoke(this, new Events.OnFloatChangedEventArgs { i = progressValue });
        }

        private void StartLoadingScene()
        {
            IsLoadingScene = true;

            OnLoadingChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsLoadingScene });
        }

        private void ResetLoadingScene()
        {
            IsLoadingScene = false;

            OnLoadingChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsLoadingScene });
        }
    }
}