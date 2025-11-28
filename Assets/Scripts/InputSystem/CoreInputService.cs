using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class CoreInputService : MonoBehaviour, InputSystem_CoreActions.IHotkeysActions, IInputService
    {
        private const string GameSpeedNormalActionKey = "GameSpeed_Normal";
        private const string GameSpeedDoubleActionKey = "GameSpeed_Double";
        private const string GameSpeedTripleActionKey = "GameSpeed_Triple";
        private const string GamePauseActionKey = "GamePause";
        private const string BuildingKey = "Building";
        private const string LeftMouseButtonKey = "LeftMouseButton";

        public event Action<float> OnGameSpeedClicked;
        public event Action OnGamePauseClicked;
        public event Action OnBuildingClicked;
        public event Action OnLeftMouseButtonClicked;
        
        
        [SerializeField] private PlayerInput _playerInput;

        private void OnEnable()
        {
            _playerInput.actions[GameSpeedNormalActionKey].canceled += OnGameSpeed_Normal;
            _playerInput.actions[GameSpeedDoubleActionKey].canceled += OnGameSpeed_Double;
            _playerInput.actions[GameSpeedTripleActionKey].canceled += OnGameSpeed_Triple;
            _playerInput.actions[GamePauseActionKey].canceled += OnGamePause;
            _playerInput.actions[BuildingKey].canceled += OnBuilding;
            _playerInput.actions[LeftMouseButtonKey].canceled += OnLeftMouseButton;
        }
        
        private void OnDisable()
        {
            _playerInput.actions[GameSpeedNormalActionKey].canceled -= OnGameSpeed_Normal;
            _playerInput.actions[GameSpeedDoubleActionKey].canceled -= OnGameSpeed_Double;
            _playerInput.actions[GameSpeedTripleActionKey].canceled -= OnGameSpeed_Triple;
            _playerInput.actions[GamePauseActionKey].canceled -= OnGamePause;
            _playerInput.actions[BuildingKey].canceled -= OnBuilding;
            _playerInput.actions[LeftMouseButtonKey].canceled -= OnLeftMouseButton;
        }
        
        public void OnGameSpeed_Normal(InputAction.CallbackContext context)
        {
            OnGameSpeedClicked?.Invoke(1f);
        }

        public void OnGameSpeed_Double(InputAction.CallbackContext context)
        {
            OnGameSpeedClicked?.Invoke(2f);
        }

        public void OnGameSpeed_Triple(InputAction.CallbackContext context)
        {
            OnGameSpeedClicked?.Invoke(3f);
        }

        public void OnGamePause(InputAction.CallbackContext context)
        {
            OnGamePauseClicked?.Invoke();
        }

        public void OnBuilding(InputAction.CallbackContext context)
        {
            OnBuildingClicked?.Invoke();
        }

        public void OnLeftMouseButton(InputAction.CallbackContext context)
        {
            OnLeftMouseButtonClicked?.Invoke();
        }
    }
}