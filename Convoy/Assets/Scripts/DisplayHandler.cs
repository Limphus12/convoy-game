using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHandler : MonoBehaviour
{
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);

        foreach (Display display in Display.displays)
        {
            display.Activate();
        }
    }
}
