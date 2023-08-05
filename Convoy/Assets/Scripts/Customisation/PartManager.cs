using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class PartManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] parts;

        // UI buttons for switching parts
        [SerializeField] private Button switchForwardButton;
        [SerializeField] private Button switchBackButton;

        // Index of the currently selected part
        private int currentPartIndex = 0;

        private GameObject currentPart;

        private void Start()
        {
            // Spawn the initial part
            SpawnPart();

            // Assign click events for UI buttons
            switchForwardButton.onClick.AddListener(SwitchForward);
            switchBackButton.onClick.AddListener(SwitchBackward);
        }

        private void SwitchForward() => SwitchPart(true);

        private void SwitchBackward() => SwitchPart(false);

        private void SwitchPart(bool forward)
        {
            // Increment or decrement the part index based on the 'forward' flag and loop back to the start/end if necessary
            if (forward) currentPartIndex = (currentPartIndex + 1) % parts.Length;
            else currentPartIndex = (currentPartIndex - 1 + parts.Length) % parts.Length;

            // Destroy the current part if any
            if (currentPart != null) Destroy(currentPart);

            // Spawn the new part
            SpawnPart();
        }

        private void SpawnPart()
        {
            // Instantiate the new part prefab at the vehicle's position and rotation
            GameObject newPart = Instantiate(parts[currentPartIndex], transform.position, transform.rotation);

            // Parent the new part to the vehicle for organization
            newPart.transform.SetParent(transform);

            // Set the new part as the current part
            currentPart = newPart;
        }
    }
}