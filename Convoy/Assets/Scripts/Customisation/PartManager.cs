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

        private void Start()
        {
            //spawn the initial part
            SpawnPart();
        }

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
            //increment or decrement the part index based on the 'forward' flag and loop back to the start/end if necessary
            if (forward) currentPartIndex = (currentPartIndex + 1) % parts.Length;
            else currentPartIndex = (currentPartIndex - 1 + parts.Length) % parts.Length;

            //spawn the new part
            SpawnPart();
        }

        private void SpawnPart()
        {
            // Destroy the current part if any
            if (currentPart != null) Destroy(currentPart);

            // Instantiate the new part prefab at the vehicle's position and rotation
            GameObject newPart = Instantiate(parts[currentPartIndex], transform.position, transform.rotation);

            // Parent the new part to the vehicle for organization
            newPart.transform.SetParent(transform);

            // Set the new part as the current part
            currentPart = newPart;
        }
    }
}