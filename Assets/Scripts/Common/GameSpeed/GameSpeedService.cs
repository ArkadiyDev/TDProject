using System.Collections.Generic;
using InputSystem;
using UnityEngine;

namespace Common.GameSpeed
{
    public class GameSpeedService : IGameSpeedService
    {
        private readonly IInputService _inputService;
        private readonly Dictionary<GameSpeed, float> _speedMap = new()
        {
            {GameSpeed.Pause, 0f},
            {GameSpeed.Normal, 1f},
            {GameSpeed.Double, 2f},
            {GameSpeed.Triple, 3f},
        };
        
        public bool IsPaused =>
            Mathf.Approximately(Time.timeScale, 0);

        public void SetPaused() =>
            SetSpeed(GameSpeed.Pause);

        public void SetUnpaused() =>
            SetSpeed(GameSpeed.Normal);

        public void SetSpeed(GameSpeed speed)
        {
            Time.timeScale = _speedMap[speed];
            Debug.Log("Game speed changed to: " + _speedMap[speed]);
        }

        public void SwitchPause() =>
            SetSpeed(IsPaused ? GameSpeed.Normal : GameSpeed.Pause);
    }
}