using InputSystem;

namespace UI
{
    public interface IInputHandler
    {
        bool HandleKeyPressed(InputIntent inputIntent);
    }
}