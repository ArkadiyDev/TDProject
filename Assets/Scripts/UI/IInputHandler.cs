using UnityEngine;

namespace UI
{
    public interface IInputHandler
    {
        bool HandleKeyPressed(KeyCode keyCode);
    }
}