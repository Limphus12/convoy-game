using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public class PartInventory : MonoBehaviour
    {
        private void Awake()
        {
            GameObject ifv_chassis = Resources.Load("Parts/Chassis/ifv_chassis.prefab", typeof(GameObject)) as GameObject;
        }
    }
}