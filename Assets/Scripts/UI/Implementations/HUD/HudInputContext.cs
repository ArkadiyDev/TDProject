using System;
using System.Collections.Generic;
using Common.GameSpeed;
using Core.Building;
using InputSystem;

namespace UI.Implementations.HUD
{
    public class HudInputContext : IInputHandler
    {
        private readonly Dictionary<InputIntent, Action> _actions = new();

        public HudInputContext(IGameSpeedService speedService, IBuildingService buildingService, Action onBackAction)
        {
            _actions[InputIntent.SpeedNormal] = () => speedService.SetSpeed(GameSpeed.Normal);
            _actions[InputIntent.SpeedDouble] = () => speedService.SetSpeed(GameSpeed.Double);
            _actions[InputIntent.SpeedTriple] = () => speedService.SetSpeed(GameSpeed.Triple);
            _actions[InputIntent.PauseSwitch] = () => speedService.SwitchPause();
            _actions[InputIntent.Escape] = () => {
                if (buildingService.IsBuildingState)
                    buildingService.SwitchBuildingMode();
                else onBackAction?.Invoke();
            };
            _actions[InputIntent.BuildMode] = () => buildingService.SwitchBuildingMode();
            _actions[InputIntent.LeftMouseClick] = () => buildingService.TryBuildTower();
        }

        public bool HandleKeyPressed(InputIntent intent)
        {
            if (!_actions.TryGetValue(intent, out var action))
                return false;
            
            action?.Invoke();
            return true;
        }
    }
}