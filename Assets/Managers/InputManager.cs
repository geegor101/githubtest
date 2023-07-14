using System;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

namespace Managers
{
    public static class InputManager
    {
        
        [CanBeNull] public static InputActionAsset _InputActionAsset { get; private set;  }

        public static void Init(InputActionAsset inputActionAsset)
        {
            _InputActionAsset = inputActionAsset;
            inputActionAsset.Enable();
        }

        public static InputAction GetInputAction(string id)
        {
            return _InputActionAsset.FindAction(id, true);
        }

        public static void QuickAddInput(string id, Action<InputAction.CallbackContext> action)
        {
            _InputActionAsset.FindAction(id, true).performed += action;
            
        }
    }
}