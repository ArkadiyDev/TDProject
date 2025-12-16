using System;
using InputSystem;
using Zenject;

namespace UI
{
    public class UIInputMediator : IInitializable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IUIService _uiService;

        public UIInputMediator(IInputService inputService, IUIService uiService)
        {
            _inputService = inputService;
            _uiService = uiService;
        }

        public void Initialize()
        {
            _inputService.OnInputActionExecuted += OnInputActionExecute;
        }

        public void Dispose()
        {
            _inputService.OnInputActionExecuted -= OnInputActionExecute;
        }

        private void OnInputActionExecute(InputIntent inputIntent)
        {
            _uiService.ProcessKeyInput(inputIntent);
        }
    }
}