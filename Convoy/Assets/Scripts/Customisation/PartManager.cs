using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class PartManager : MonoBehaviour
    {
        [SerializeField] protected GameObject[] parts;

        //index of the currently selected part
        protected int currentPartIndex = 0;

        protected GameObject currentPart;

        public bool hasSpawnedFirstPart = false;

        public static event EventHandler<EventArgs> OnPartChangedEvent;

        protected void OnPartChanged() => OnPartChangedEvent?.Invoke(this, EventArgs.Empty);
        public int GetCurrentPartIndex() => currentPartIndex;

        public void SetPart(int partIndex)
        {
            //set the current part, using the provided index
            currentPartIndex = partIndex;

            //spawn the new part
            SpawnPart();
        }

        public void SwitchForward() => SwitchPart(true);

        public void SwitchBackward() => SwitchPart(false);

        private void SwitchPart(bool forward)
        {
            //increment or decrement the part index
            if (forward && currentPartIndex < parts.Length - 1) currentPartIndex++;
            else if (!forward && currentPartIndex > 0) currentPartIndex--;

            //spawn the new part
            SpawnPart();
        }

        protected void SpawnPart()
        {
            //destroy the current part if any
            if (currentPart != null) Destroy(currentPart);

            //instantiate the new part prefab at the vehicle's position and rotation
            GameObject newPart = Instantiate(parts[currentPartIndex], transform.position + parts[currentPartIndex].transform.position, parts[currentPartIndex].transform.rotation, transform);

            //set the new part as the current part
            currentPart = newPart;

            OnPartChanged();
        }
    }
}