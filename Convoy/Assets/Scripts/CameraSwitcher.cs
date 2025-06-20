using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class CameraSwitcher : MonoBehaviour
    {
        public Camera MainCamera;

        public RenderTexture renderTexture;

        public static Camera POVCamera;

        bool isSwitched = false;

        public static event EventHandler<Events.BoolEventArgs> OnCameraSwitchedEvent;
        public static event EventHandler<EventArgs> OnPOVCameraChangedEvent;

        protected void OnCameraSwitched() => OnCameraSwitchedEvent?.Invoke(this, new Events.BoolEventArgs { i = isSwitched });
        protected static void OnPOVCameraChanged() => OnPOVCameraChangedEvent?.Invoke(typeof(CameraSwitcher), EventArgs.Empty);

        private void Awake()
        {
            UIManager.OnCameraToggledEvent += UIManager_OnCameraToggledEvent;
            VehicleCamera.OnCameraToggledEvent += VehicleCamera_OnCameraToggledEvent;
        }

        private void OnDestroy()
        {
            UIManager.OnCameraToggledEvent -= UIManager_OnCameraToggledEvent;
        }

        private void TargetSystem_OnNoTargetSelectedEvent(object sender, EventArgs e)
        {   
            if (!isSwitched) SetPOVCamera(null);
        }

        private void UIManager_OnCameraToggledEvent(object sender, EventArgs e)
        {
            SwitchCamera();
        }

        public void SwitchCamera()
        {
            isSwitched = !isSwitched;

            //not switched, keep render texture on pov
            if (!isSwitched)
            {
                MainCamera.targetTexture = null;
                POVCamera.targetTexture = renderTexture;
            }

            //switched, move render texture to main
            else if (isSwitched)
            {
                MainCamera.targetTexture = renderTexture;
                POVCamera.targetTexture = null;
            }

            OnCameraSwitched();
        }

        private void VehicleCamera_OnCameraToggledEvent(object sender, Events.GameObjectEventArgs e)
        {
            if (e.i.activeInHierarchy) 
            { 
                SetPOVCamera(e.i.GetComponent<Camera>()); SetPOVCameraTargetTexture(renderTexture); 
            }
        }

        protected void SetPOVCamera(Camera camera) => POVCamera = camera;
        protected void SetPOVCameraTargetTexture(RenderTexture texture) => POVCamera.targetTexture = texture;
    }
}