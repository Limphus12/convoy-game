using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.save_system;

namespace com.limphus.convoy
{
    public class PartInventory : MonoBehaviour
    {
        [Header("Chassis Prefabs")]
        [SerializeField] private GameObject ifvChassisPrefab;
        [SerializeField] private GameObject lrfChassisPrefab;

        [Header("Turret Prefabs")]
        [SerializeField] private GameObject ifvMGTurretPrefab;
        [SerializeField] private GameObject ifvMortarTurretPrefab;

        [Space]
        [SerializeField] private GameObject lrfMGTurretPrefab;
        [SerializeField] private GameObject lrfMortarTurretPrefab;


        [Space]
        [SerializeField] private int[,] chassisAndCounts;
        [SerializeField] private int[,] turretsAndCounts;

        private void Awake()
        {
            //GameObject ifv_chassis = Resources.Load("Parts/Chassis/ifv_chassis.prefab", typeof(GameObject)) as GameObject;


        }
    }
}