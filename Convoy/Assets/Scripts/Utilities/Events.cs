using System;
using UnityEngine;

namespace com.limphus.utilities
{
    public class Events
    {
        public class IntEventArgs : EventArgs { public int i; }
        public class FloatEventArgs : EventArgs { public float i; }
        public class BoolEventArgs : EventArgs { public bool i; }
        public class StringEventArgs : EventArgs { public string i; }
        public class CharEventArgs : EventArgs { public char i; }
        public class DoubleEventArgs : EventArgs { public double i; }
        public class RaycastHitEventArgs : EventArgs { public RaycastHit i; }
        public class GameObjectEventArgs : EventArgs { public GameObject i; }
        public class Vector3EventArgs : EventArgs { public Vector3 i; }
    }
}