using System;
using InputSystem;

namespace UI.Implementations.Popup
{
    public class PopupMediator : UIPopupMediator<PopupView>, IInputHandler
    {
        private readonly PopupInputContext _inputContext;
        
        public event Action DialogButtonPressed;
        
        public PopupMediator(PopupView view) : base(view)
        {
            _inputContext = new PopupInputContext(CloseRequest);
        }
        
        public override void Show()
        {
            base.Show();
            
            _view.DialogButton.onClick.AddListener(DialogButtonPress);
        }

        public override void Hide()
        {
            base.Hide();
            
            _view.DialogButton.onClick.RemoveListener(DialogButtonPress);
        }

        public bool HandleKeyPressed(InputIntent inputIntent) =>
            _inputContext.HandleKeyPressed(inputIntent);

        private void DialogButtonPress()
        {
            DialogButtonPressed?.Invoke();
        }
    }
}