using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class Billboard : MonoBehaviour
    {
        private Transform cam;

        private void Awake()
        {
            if (!cam) cam = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}