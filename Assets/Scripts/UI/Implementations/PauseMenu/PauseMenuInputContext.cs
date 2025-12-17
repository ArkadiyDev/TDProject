using System;
using System.Collections.Generic;
using InputSystem;

namespace UI.Implementations.PauseMenu
{
    public class PauseMenuInputContext : IInputHandler
    {
        private readonly Dictionary<InputIntent, Action> _actions = new();

        public PauseMenuInputContext(Action onBackAction)
        {
            _actions[InputIntent.Escape] = () => onBackAction?.Invoke();
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