using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Implementations.HUD
{
    public class HudView : UIView
    {
        [SerializeField] private TMP_Text _goldCounter;
        [SerializeField] private Button _popupButton;
        [SerializeField] private Button _dialogButton;

        public Button PopupButton => _popupButton;
        public Button DialogButton => _dialogButton;
        
        public void SetGoldCounter(int value)
        {
            _goldCounter.SetText(value.ToString());
        }
    }
}