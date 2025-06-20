using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class TargetSystemUI : MonoBehaviour
    {
        [SerializeField] private WorldFollow playerSelectedTargetUI, enemySelectedTargetUI;

        [SerializeField] private GameObject targetCanvas;
        [SerializeField] private WorldFollow playerTargetUI, enemyTargetUI;

        [Space, SerializeField] private Color playerFlirColor;
        [SerializeField] private Color enemyFlirColor;

        [Space, SerializeField] private Color playerOptColor;
        [SerializeField] private Color enemyOptColor;

        private static List<WorldFollow> playerWorldFollows = new List<WorldFollow>();
        private static List<WorldFollow> enemyWorldFollows = new List<WorldFollow>();

        bool canUpdate = true;

        private Color playerCurrentColor, enemyCurrentColor;

        private Color GetCurrentColorByTargetType(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Player:
                    return playerCurrentColor;

                case TargetType.Enemy:
                    return enemyCurrentColor;

                default:
                    return Color.white;
            }
        }

        private void Awake()
        {
            CameraSwitcher.OnCameraSwitchedEvent += CameraSwitcher_OnCameraSwitchedEvent;
            UIManager.OnFlirToggledEvent += UIManager_OnFlirToggledEvent;

            playerCurrentColor = playerFlirColor;
            enemyCurrentColor = enemyFlirColor;
        }

        private void OnDestroy()
        {
            CameraSwitcher.OnCameraSwitchedEvent -= CameraSwitcher_OnCameraSwitchedEvent;
            UIManager.OnFlirToggledEvent -= UIManager_OnFlirToggledEvent;
        }

        private void UIManager_OnFlirToggledEvent(object sender, Events.BoolEventArgs e)
        {
            ToggleWorldFollowColours(e.i);
        }

        private void ToggleWorldFollowColours(bool b)
        {
            if (b) { playerCurrentColor = playerFlirColor; enemyCurrentColor = enemyFlirColor; }
            else if (!b) { playerCurrentColor = playerOptColor; enemyCurrentColor = enemyOptColor; }

            foreach (WorldFollow worldFollow in playerWorldFollows)
            {
                if (worldFollow == null) continue;
                worldFollow.GetComponent<RawImage>().color = playerCurrentColor;
            }

            foreach (WorldFollow worldFollow in enemyWorldFollows)
            {
                if (worldFollow == null) continue;
                worldFollow.GetComponent<RawImage>().color = enemyCurrentColor; 
            }
        }

        private void CameraSwitcher_OnCameraSwitchedEvent(object sender, Events.BoolEventArgs e)
        {
            ToggleWorldFollows(!e.i);

            canUpdate = !e.i;
        }

        private void ToggleWorldFollows(bool b)
        {
            foreach (WorldFollow worldFollow in playerWorldFollows)
            {
                if (worldFollow) worldFollow.gameObject.SetActive(b);
            }

            foreach (WorldFollow worldFollow in enemyWorldFollows)
            {
                if (worldFollow) worldFollow.gameObject.SetActive(b);
            }
        }

        void LateUpdate()
        {
            if (canUpdate)
            {
                TargetUI();

                SelectedTargetUI(playerSelectedTargetUI, TargetType.Player);
                SelectedTargetUI(enemySelectedTargetUI, TargetType.Enemy);
            }
        }

        private void TargetUI()
        {
            if (targetCanvas)
            {
                if (enemyTargetUI)
                {
                    int i = 0;

                    foreach (Target target in TargetSystem.visibleEnemyTargets)
                    {
                        //if the target already has a ui thingy, just move onto the next one.
                        if (target == null) continue;
                        if (target.GetComponentInChildren<WorldFollow>()) continue;

                        GameObject canvasGO = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(enemyTargetUI, canvasGO.transform);

                        ui.GetComponent<RawImage>().color = enemyCurrentColor;

                        ui.SetTarget(target.transform);

                        enemyWorldFollows.Add(ui);

                        i++;
                    }
                }

                if (playerTargetUI)
                {
                    int i = 0;

                    foreach (Target target in TargetSystem.visiblePlayerTargets)
                    {
                        //if the target already has a ui thingy, just move onto the next one.
                        if (target == null) continue;
                        if (target.GetComponentInChildren<WorldFollow>()) continue;

                        GameObject canvasGO = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(playerTargetUI, canvasGO.transform);

                        ui.GetComponent<RawImage>().color = playerCurrentColor;

                        ui.SetTarget(target.transform);

                        playerWorldFollows.Add(ui);

                        i++;
                    }
                }
            }
        }

        private void SelectedTargetUI(WorldFollow selectedTargetUI, TargetType targetType)
        {
            if (!selectedTargetUI) return;

            Target tg = TargetSystem.GetSelectedTargetByType(targetType);

            if (!tg) selectedTargetUI.gameObject.SetActive(false);

            else if (tg)
            {
                selectedTargetUI.gameObject.SetActive(true);

                selectedTargetUI.GetComponent<RawImage>().color = GetCurrentColorByTargetType(targetType);

                selectedTargetUI.SetTarget(tg.transform);
            }

            //if (!TargetSystem.selectedTarget)
            //{
            //    selectedTargetUI.gameObject.SetActive(false);
            //}
            //
            //else if (TargetSystem.selectedTarget)
            //{
            //    selectedTargetUI.gameObject.SetActive(true);
            //
            //    if (TargetSystem.selectedTarget.GetTargetType == TargetType.Player)
            //    {
            //        selectedTargetUI.GetComponent<RawImage>().color = playerCurrentColor;
            //    }
            //
            //    else if (TargetSystem.selectedTarget.GetTargetType == TargetType.Enemy)
            //    {
            //        selectedTargetUI.GetComponent<RawImage>().color = enemyCurrentColor;
            //    }
            //
            //    selectedTargetUI.SetTarget(TargetSystem.selectedTarget.transform);
            //}
        }
    }
}