using InputSystem;
using UnityEngine;

namespace Common.GameSpeed
{
    public class GameSpeedService : IGameSpeedService
    {
        private readonly IInputService _inputService;
        
        public bool IsPaused => Mathf.Approximately(Time.timeScale, 0);

        public GameSpeedService(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.OnGameSpeedClicked += OnGameSpeedClicked;
            _inputService.OnGamePauseClicked += OnGamePauseClicked;
        }

        public void SetPaused()
        {
            SetSpeed(0);
        }

        private void OnGameSpeedClicked(float speed)
        {
            SetSpeed(speed);
        }

        private void OnGamePauseClicked()
        {
            SetSpeed(IsPaused ? 0 : 1);
        }
        
        private void SetSpeed(float speed)
        {
            Time.timeScale = speed;
            Debug.Log("Game speed changed to: " + speed);
        }
    }
}