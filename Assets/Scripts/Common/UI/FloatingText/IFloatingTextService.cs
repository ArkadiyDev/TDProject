using UnityEngine;

namespace Common.UI.FloatingText
{
    public interface IFloatingTextService
    {
        void ShowText(Vector3 position, string text);
    }
}