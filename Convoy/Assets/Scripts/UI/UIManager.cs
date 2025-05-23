using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteCanvas, levelFailCanvas;

        private void Awake()
        {
            GameManager.OnLevelCompleteEvent += GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent += GameManager_OnLevelFailEvent;
        }

        private void OnDestroy()
        {
            GameManager.OnLevelCompleteEvent -= GameManager_OnLevelCompleteEvent;
            GameManager.OnLevelFailEvent -= GameManager_OnLevelFailEvent;
        }

        private void GameManager_OnLevelCompleteEvent(object sender, System.EventArgs e)
        {
            levelCompleteCanvas.SetActive(true);
        }

        private void GameManager_OnLevelFailEvent(object sender, System.EventArgs e)
        {
            levelFailCanvas.SetActive(true);
        }
    }
}