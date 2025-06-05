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
            if (levelCompleteUI) levelCompleteUI.SetActive(true);
        }

        private void GameManager_OnLevelFailEvent(object sender, System.EventArgs e)
        {
            if (levelFailUI) levelFailUI.SetActive(true);
        }

        private void PauseManager_OnPausedChangedEvent(object sender, System.EventArgs e)
        {
            if (PauseManager.IsPaused)
            {
                if (gameUI) gameUI.SetActive(false);
                if (menuUI) menuUI.SetActive(false);

                if (pauseUI) pauseUI.SetActive(true);
            }

            else if (!PauseManager.IsPaused)
            {
                ResetGameUI();
            }
        }

        private void ResetGameUI()
        {
            if (optionsMenu) optionsMenu.SetActive(false);
            if (controlsPanel) controlsPanel.SetActive(false);
            if (videoPanel) videoPanel.SetActive(false);
            if (audioPanel) audioPanel.SetActive(false);

            if (gameUI) gameUI.SetActive(true);
            if (menuUI) menuUI.SetActive(true);

            if (pauseUI) pauseUI.SetActive(false);
        }
    }
}