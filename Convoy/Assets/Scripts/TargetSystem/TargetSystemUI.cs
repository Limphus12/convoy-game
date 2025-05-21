using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class TargetSystemUI : MonoBehaviour
    {
        [SerializeField] private WorldFollow selectedTargetUI;

        [SerializeField] private GameObject targetCanvas;
        [SerializeField] private WorldFollow playerTargetUI, enemyTargetUI;

        void Update()
        {
            SelectedTargetUI(); TargetUI();
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

                        WorldFollow worldFollow = target.GetComponentInChildren<WorldFollow>();

                        if (worldFollow != null)
                        {
                            //destroys the original target canvas if we have selected (no overlapping ui).
                            if (target == TargetSystem.playerSelectedTarget)
                            {
                                Canvas canvas1 = worldFollow.GetComponentInParent<Canvas>();

                                if (canvas1 != null) Destroy(canvas1.gameObject);
                            }

                            continue;
                        }

                        GameObject canvas = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(enemyTargetUI, canvas.transform);

                        ui.SetTarget(target.transform);

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

                        GameObject canvas = Instantiate(targetCanvas, target.transform);

                        WorldFollow ui = Instantiate(playerTargetUI, canvas.transform);

                        ui.SetTarget(target.transform);

                        i++;
                    }
                }
            }
        }

        private void SelectedTargetUI()
        {
            if (!TargetSystem.playerSelectedTarget && selectedTargetUI)
            {
                selectedTargetUI.gameObject.SetActive(false);
            }

            else if (TargetSystem.playerSelectedTarget && selectedTargetUI)
            {
                selectedTargetUI.gameObject.SetActive(true);

                selectedTargetUI.SetTarget(TargetSystem.playerSelectedTarget.transform);
            }
        }
    }
}