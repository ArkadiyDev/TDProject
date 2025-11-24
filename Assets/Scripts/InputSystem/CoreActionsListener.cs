using Common.GameSpeed;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class CoreActionsListener : MonoBehaviour, InputSystem_CoreActions.IHotkeysActions
    {
        private const string GameSpeedNormalActionKey = "GameSpeed_Normal";
        private const string GameSpeedDoubleActionKey = "GameSpeed_Double";
        private const string GameSpeedTripleActionKey = "GameSpeed_Triple";
        private const string GamePauseActionKey = "GamePause";
        
        [SerializeField] private PlayerInput _playerInput;
        
        private IGameSpeedService _gameSpeedService;

        [Inject]
        private void Construct(IGameSpeedService gameSpeedService)
        {
            _gameSpeedService = gameSpeedService;
        }

        private void OnEnable()
        {
            _playerInput.actions[GameSpeedNormalActionKey].canceled += OnGameSpeed_Normal;
            _playerInput.actions[GameSpeedDoubleActionKey].canceled += OnGameSpeed_Double;
            _playerInput.actions[GameSpeedTripleActionKey].canceled += OnGameSpeed_Triple;
            _playerInput.actions[GamePauseActionKey].canceled += OnGamePause;
        }
        
        private void OnDisable()
        {
            _playerInput.actions[GameSpeedNormalActionKey].canceled -= OnGameSpeed_Normal;
            _playerInput.actions[GameSpeedDoubleActionKey].canceled -= OnGameSpeed_Double;
            _playerInput.actions[GameSpeedTripleActionKey].canceled -= OnGameSpeed_Triple;
            _playerInput.actions[GamePauseActionKey].canceled -= OnGamePause;
        }
        
        public void OnGameSpeed_Normal(InputAction.CallbackContext context)
        {
            _gameSpeedService.SetNormalSpeed();
        }

        public void OnGameSpeed_Double(InputAction.CallbackContext context)
        {
            _gameSpeedService.SetDoubleSpeed();
        }

        public void OnGameSpeed_Triple(InputAction.CallbackContext context)
        {
            _gameSpeedService.SetTripleSpeed();
        }

        public void OnGamePause(InputAction.CallbackContext context)
        {
            _gameSpeedService.SetPaused(!_gameSpeedService.IsPaused);
        }
    }
}