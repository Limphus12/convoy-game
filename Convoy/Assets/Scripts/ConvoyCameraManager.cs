using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace com.limphus.convoy
{
    public class ConvoyCameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;

        private void Start()
        {
            Transform[] transforms = new Transform[ConvoyManager.vehiclesList.Count];

            for (int i = 0; i < transforms.Length; i++)
            {
                if (ConvoyManager.vehiclesList[i] == null) continue;

                transforms[i] = ConvoyManager.vehiclesList[i].transform;
            }

            SetTargets(transforms);
        }

        public void SetTargets(Transform[] transforms)
        {
            targetGroup.m_Targets = new CinemachineTargetGroup.Target[transforms.Length];

            for (int i = 0; i < targetGroup.m_Targets.Length; i++)
            {
                targetGroup.m_Targets[i].target = transforms[i];
                targetGroup.m_Targets[i].weight = 1;
            }
        }
    }
}