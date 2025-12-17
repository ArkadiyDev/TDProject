using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class CoreInputService : MonoBehaviour, IInputService
    {
        public event Action<InputIntent> OnInputActionExecuted;
        
        [SerializeField] private PlayerInput _playerInput;
        
        private readonly Dictionary<string, InputIntent> _commandMap = new()
            {
                { "GamePause", InputIntent.PauseSwitch },
                { "GameSpeed_Normal", InputIntent.SpeedNormal },
                { "GameSpeed_Double", InputIntent.SpeedDouble },
                { "GameSpeed_Triple", InputIntent.SpeedTriple },
                { "Building", InputIntent.BuildMode },
                { "Escape", InputIntent.Escape },
                { "LeftMouseButton", InputIntent.LeftMouseClick },
            };
        
        private void OnEnable() =>
            _playerInput.onActionTriggered += HandlePerformedAction;
        
        private void OnDisable() =>
            _playerInput.onActionTriggered -= HandlePerformedAction;
        
        private void HandlePerformedAction(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (_commandMap.TryGetValue(context.action.name, out var inputIntent))
                OnInputActionExecuted?.Invoke(inputIntent);
        }
    }
}