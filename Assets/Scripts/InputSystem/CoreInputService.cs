using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class CoreInputService : MonoBehaviour, InputSystem_CoreActions.IHotkeysActions, IInputService
    {
        public event Action<float> OnGameSpeedClicked;
        public event Action OnGamePauseClicked;
        public event Action OnBuildingClicked;
        public event Action OnLeftMouseButtonClicked;

        private InputSystem_CoreActions _controls;

        private void Awake()
        {
            _controls = new InputSystem_CoreActions();
            
            _controls.Hotkeys.SetCallbacks(this);
        }
        
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
        
        public void OnGameSpeed_Normal(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnGameSpeedClicked?.Invoke(1f);
        }

        public void OnGameSpeed_Double(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnGameSpeedClicked?.Invoke(2f);
        }

        public void OnGameSpeed_Triple(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnGameSpeedClicked?.Invoke(3f);
        }

        public void OnGamePause(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnGamePauseClicked?.Invoke();
        }

        public void OnBuilding(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnBuildingClicked?.Invoke();
        }

        public void OnLeftMouseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnLeftMouseButtonClicked?.Invoke();
        }
    }
}