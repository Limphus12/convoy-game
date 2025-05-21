using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class InputManager : MonoBehaviour
    {
        public static event EventHandler<EventArgs> OnMiddleMouseDownEvent;

        protected void OnMiddleMouseDown() => OnMiddleMouseDownEvent?.Invoke(this, EventArgs.Empty);

        void Update()
        {
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (Input.GetMouseButtonDown(2)) OnMiddleMouseDown();
        }
    }
}