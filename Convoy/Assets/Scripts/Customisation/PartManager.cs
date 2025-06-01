using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class PartManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] parts;

        //index of the currently selected part
        private int currentPartIndex = 0;

        private GameObject currentPart;

        public int GetCurrentPartIndex() => currentPartIndex;

        private void Start()
        {
            SpawnPart(); //spawn the initial part
        }

        public void SetPartIndex(int partIndex) => currentPartIndex = partIndex;

        public void SetPart(int partIndex)
        {
            //set the current part, using the provided index
            currentPart = parts[partIndex];

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

        private void SpawnPart()
        {
            //destroy the current part if any
            if (currentPart != null) Destroy(currentPart);

            //instantiate the new part prefab at the vehicle's position and rotation
            GameObject newPart = Instantiate(parts[currentPartIndex], transform.position + parts[currentPartIndex].transform.position, parts[currentPartIndex].transform.rotation, transform);

            //set the new part as the current part
            currentPart = newPart;
        }
    }
}