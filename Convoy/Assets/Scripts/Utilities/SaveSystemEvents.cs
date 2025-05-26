using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.save_system
{
    public class SaveSystemEvents : Events
    {
        public class OnSettingsChangedEventArgs : EventArgs { public SettingsData i; }
    }
}