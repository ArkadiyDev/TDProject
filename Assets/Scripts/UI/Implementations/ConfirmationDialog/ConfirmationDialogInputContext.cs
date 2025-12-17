using System;
using System.Collections.Generic;
using InputSystem;

namespace UI.Implementations.ConfirmationDialog
{
    public class ConfirmationDialogInputContext : IInputHandler
    {
        private readonly Dictionary<InputIntent, Action> _actions = new();

        public ConfirmationDialogInputContext(Action onBackAction)
        {
            _actions[InputIntent.Escape] = () => onBackAction?.Invoke();
        }

        public bool HandleKeyPressed(InputIntent intent)
        {
            if (_actions.TryGetValue(intent, out var action))
                action?.Invoke();
            
            return true;
        }
    }
}