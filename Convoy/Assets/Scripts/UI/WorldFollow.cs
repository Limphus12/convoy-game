using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class WorldFollow : MonoBehaviour
    {
        [SerializeField] private Transform targetLookAt;
        [SerializeField] private Vector3 offset;

        private Camera cam;

        public void SetTarget(Transform target)
        {
            if (targetLookAt == target) return;

            targetLookAt = target;
        }

        private void Awake()
        {
            if (!cam) cam = Camera.main;
        }

        void LateUpdate()
        {
            if (targetLookAt == null) return;

            Vector3 pos = cam.WorldToScreenPoint(targetLookAt.position + offset); transform.position = pos;
        }
    }
}