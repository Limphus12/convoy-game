using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class VehicleCamera : MonoBehaviour
    {
        public Camera cam;

        public RenderTexture renderTexture;

        public static event EventHandler<Events.GameObjectEventArgs> OnCameraToggledEvent;

        protected void OnCameraToggled(GameObject cam) => OnCameraToggledEvent?.Invoke(this, new Events.GameObjectEventArgs { i = cam.gameObject });
        
        private void Awake()
        {
            PartManager.OnPartChangedEvent += PartManager_OnPartChangedEvent;
            TargetSystem.OnPlayerTargetSelectedEvent += TargetSystem_OnPlayerTargetSelectedEvent;
            TargetSystem.OnPlayerTargetDeSelectedEvent += TargetSystem_OnPlayerTargetDeSelectedEvent;
        }

        private void OnDestroy()
        {
            PartManager.OnPartChangedEvent -= PartManager_OnPartChangedEvent;
            TargetSystem.OnPlayerTargetSelectedEvent -= TargetSystem_OnPlayerTargetSelectedEvent;
            TargetSystem.OnPlayerTargetDeSelectedEvent -= TargetSystem_OnPlayerTargetDeSelectedEvent;
        }

        private void TargetSystem_OnPlayerTargetSelectedEvent(object sender, EventArgs e)
        {
            Target target = GetComponentInChildren<Target>();

            if (target) ToggleCam(TargetSystem.playerSelectedTarget == target);
        }

        private void TargetSystem_OnPlayerTargetDeSelectedEvent(object sender, EventArgs e)
        {
            ToggleCam(false);
        }

        private void PartManager_OnPartChangedEvent(object sender, EventArgs e)
        {
            cam = GetComponentInChildren<Camera>(true);

            ToggleCam(false);
        }

        public void ToggleCam(bool on) 
        {
            if (!cam) return;

            if (on)
            {
                cam.gameObject.SetActive(on);
                if (renderTexture) cam.targetTexture = renderTexture;
            }

            else if (!on)
            {
                cam.targetTexture = null;
                cam.gameObject.SetActive(on);
            }

            OnCameraToggled(cam.gameObject);
        }
    }
}