using System;

namespace InputSystem
{
    public interface IInputService
    {
        public event Action<float> OnGameSpeedClicked;
        public event Action OnGamePauseClicked;
        public event Action OnBuildingClicked;
        public event Action OnLeftMouseButtonClicked;
    }
}