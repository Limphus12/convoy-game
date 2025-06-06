using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.limphus.convoy
{
    public class TurretManager : PartManager
    {
        private void Start()
        {
            if (!hasSpawnedFirstPart)
            {
                SpawnPart();
                hasSpawnedFirstPart = true;
            }
        }
    }
}