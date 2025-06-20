using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.save_system
{
    public class SaveSystemEvents : Events
    {
        public class OnGameChangedEventArgs : EventArgs { public GameData i; }
        public class OnSettingsChangedEventArgs : EventArgs { public SettingsData i; }
        public class OnConvoyChangedEventArgs : EventArgs { public ConvoyData i; }
    }
}