using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public enum TargetType { Player, Enemy }

    public class Target : MonoBehaviour
    {
        [SerializeField] private TargetType type;

        public TargetType GetTargetType => type;
    }
}