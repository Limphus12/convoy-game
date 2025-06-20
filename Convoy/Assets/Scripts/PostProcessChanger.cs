using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class PostProcessChanger : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        private void Awake()
        {
            UIManager.OnFlirToggledEvent += UIManager_OnFlirToggledEvent;
        }

        private void OnDestroy()
        {
            UIManager.OnFlirToggledEvent -= UIManager_OnFlirToggledEvent;
        }

        private void UIManager_OnFlirToggledEvent(object sender, Events.BoolEventArgs e)
        {
            ColorAdjustments adjustments;

            if (!volume.profile.TryGet(out adjustments)) return;

            if (e.i) adjustments.saturation.value = -100f;
            else if (!e.i) adjustments.saturation.value = 0f;
        }
    }
}