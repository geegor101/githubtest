using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public static class InputManager
    {
        private static string _focus;
        private const string DebugID = "DEBUG";

        public static string Focus
        {
            get => _focus;
            set
            {
                if (_focus == value)
                    return;
                SetFocus(value);
                _focus = value;
            }
        }

        private static InputActionAsset InputActionAsset { get; set;  }

        public static void Init(InputActionAsset inputActionAsset)
        {
            InputActionAsset = inputActionAsset;
            inputActionAsset.Enable();
            Focus = "UI";
            FocusChangedEvent += CursorLocker;
        }

        private static void CursorLocker(string oldFocus, string newFocus)
        {
            Cursor.lockState = newFocus == "GAME" ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public static InputAction GetInputAction(string id)
        {
            return InputActionAsset.FindAction(id, true);
        }

        public static void QuickAddInput(string id, Action<InputAction.CallbackContext> action)
        {
            InputActionAsset.FindAction(id, true).performed += action;
        }

        public delegate void OnFocusChanged(string oldFocus, string newFocus);

        public static event OnFocusChanged FocusChangedEvent;
        
        private static void SetFocus(string focus)
        {
            FocusChangedEvent?.Invoke(_focus, focus);
            if (InputActionAsset == null)
                return;
            foreach (InputActionMap actionMap in InputActionAsset.actionMaps)
            {
                //Debug.Log($"{focus} : {actionMap.name}, {focus == DebugID}, {focus == actionMap.name}, " + $"{focus == DebugID || focus == actionMap.name}");
                if (focus == actionMap.name || DebugID == actionMap.name)
                {
                    actionMap.Enable();
                }
                else
                {
                    actionMap.Disable();
                }
            }
        }
    }
}