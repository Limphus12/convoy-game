using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteUI, levelFailUI, gameUI, menuUI, pauseUI;

        [Space]
        [SerializeField] private GameObject optionsMenu, controlsPanel, videoPanel, audioPanel;

        private void Awake()
        {
            GameManager.OnLevelCompleteEvent += GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent += GameManager_OnLevelFailEvent;
            PauseManager.OnPausedChangedEvent += PauseManager_OnPausedChangedEvent;
        }

        private void OnDestroy()
        {
            GameManager.OnLevelCompleteEvent -= GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent -= GameManager_OnLevelFailEvent;
            PauseManager.OnPausedChangedEvent -= PauseManager_OnPausedChangedEvent;
        }

        private void GameManager_OnLevelCompleteEvent(object sender, System.EventArgs e)
        {
            levelCompleteUI.SetActive(true);
        }

        private void GameManager_OnLevelFailEvent(object sender, System.EventArgs e)
        {
            levelFailUI.SetActive(true);
        }

        private void PauseManager_OnPausedChangedEvent(object sender, System.EventArgs e)
        {
            if (PauseManager.IsPaused)
            {
                gameUI.SetActive(false);
                menuUI.SetActive(false);

                pauseUI.SetActive(true);
            }

            else if (!PauseManager.IsPaused)
            {
                ResetGameUI();
            }
        }

        private void ResetGameUI()
        {
            optionsMenu.SetActive(false);
            controlsPanel.SetActive(false);
            videoPanel.SetActive(false);
            audioPanel.SetActive(false);

            gameUI.SetActive(true);
            menuUI.SetActive(true);

            pauseUI.SetActive(false);
        }
    }
}