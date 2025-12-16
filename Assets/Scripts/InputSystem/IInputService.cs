using System;

namespace InputSystem
{
    public interface IInputService
    {
        event Action<InputIntent> OnInputActionExecuted;
    }
}