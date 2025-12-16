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
            _inputService.OnInputActionExecuted += OnInputActionExecute;
        }

        private void OnInputActionExecute(InputIntent inputIntent)
        {
            switch (inputIntent)
            {
                case InputIntent.PauseSwitch:
                    SwitchPause();
                    break;
                case InputIntent.SpeedNormal:
                    SetSpeed(1f);
                    break;
                case InputIntent.SpeedDouble:
                    SetSpeed(2f);
                    break;
                case InputIntent.SpeedTriple:
                    SetSpeed(3f);
                    break;
            }
        }

        public void SetPaused() =>
            SetSpeed(0);

        private void SwitchPause() =>
            SetSpeed(IsPaused ? 0 : 1);

        private void SetSpeed(float speed)
        {
            Time.timeScale = speed;
            Debug.Log("Game speed changed to: " + speed);
        }
    }
}