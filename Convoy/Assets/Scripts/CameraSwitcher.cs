using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

public class CameraSwitcher : MonoBehaviour
{
    public Camera MainCamera;

    public RenderTexture renderTexture;

    public static Camera POVCamera;

    bool isSwitched = false;

    public static event EventHandler<Events.BoolEventArgs> OnCameraSwitchedEvent;

    protected void OnCameraSwitched() => OnCameraSwitchedEvent?.Invoke(this, new Events.BoolEventArgs { i = isSwitched });

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

    public static void SetPOVCamera(Camera camera) => POVCamera = camera;
}
