using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject[] turretTypes;
    [SerializeField] private GameObject currentTurret;

    // Index of the currently selected turret type
    private int currentTurretIndex = 0;

    // UI buttons for switching turret types
    [SerializeField] private Button switchTurretButton;
    [SerializeField] private Button switchTurretBackButton;

    private void Start()
    {
        // Spawn the initial turret type
        SpawnTurret();

        // Assign click events for UI buttons
        switchTurretButton.onClick.AddListener(SwitchTurretForward);
        switchTurretBackButton.onClick.AddListener(SwitchTurretBackward);
    }

    private void SwitchTurretForward() => SwitchTurret(true);

    private void SwitchTurretBackward() => SwitchTurret(false);

    private void SwitchTurret(bool forward)
    {
        // Increment or decrement the turret index based on the 'forward' flag and loop back to the start/end if necessary
        if (forward) currentTurretIndex = (currentTurretIndex + 1) % turretTypes.Length;
        else currentTurretIndex = (currentTurretIndex - 1 + turretTypes.Length) % turretTypes.Length;

        // Destroy the current turret if any
        if (currentTurret != null) Destroy(currentTurret);

        // Spawn the new turret type
        SpawnTurret();
    }

    private void SpawnTurret()
    {
        // Instantiate the new turret prefab at the vehicle's position and rotation
        GameObject newTurret = Instantiate(turretTypes[currentTurretIndex], transform.position, transform.rotation);

        // Parent the new turret to the vehicle for organization
        newTurret.transform.SetParent(transform);

        // Set the new turret as the current turret
        currentTurret = newTurret;
    }
}
