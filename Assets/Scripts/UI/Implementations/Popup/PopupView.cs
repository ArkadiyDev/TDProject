using UnityEngine;
using UnityEngine.UI;

namespace UI.Implementations.Popup
{
    public class PopupView : UIView
    {
        [SerializeField] private Button _dialogButton;

        public Button DialogButton => _dialogButton;
    }
}