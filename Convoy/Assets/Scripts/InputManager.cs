using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    public class InputManager : MonoBehaviour
    {
        public static event EventHandler<EventArgs> OnLeftMouseDownEvent, OnMiddleMouseDownEvent, OnEscapeKeyDownEvent;

        protected void OnLeftMouseDown() => OnLeftMouseDownEvent?.Invoke(this, EventArgs.Empty);
        protected void OnMiddleMouseDown() => OnMiddleMouseDownEvent?.Invoke(this, EventArgs.Empty);
        protected void OnEscapeKeyDown() => OnEscapeKeyDownEvent?.Invoke(this, EventArgs.Empty);

        void Update()
        {
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (Input.GetMouseButtonDown(0)) OnLeftMouseDown();

            if (Input.GetMouseButtonDown(2)) OnMiddleMouseDown();

            if (Input.GetKeyDown(KeyCode.Escape)) OnEscapeKeyDown();
        }
    }
}