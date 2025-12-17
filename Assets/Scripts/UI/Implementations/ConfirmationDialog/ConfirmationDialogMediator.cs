using System;
using InputSystem;

namespace UI.Implementations.ConfirmationDialog
{
    public class ConfirmationDialogMediator : UIDialogMediator<ConfirmationDialogView>, IInputHandler
    {
        private readonly ConfirmationDialogInputContext _inputContext;
        
        private Action _onConfirmCallback;
        private Action _onCancelCallback;

        public ConfirmationDialogMediator(ConfirmationDialogView view) : base(view) =>
            _inputContext = new ConfirmationDialogInputContext(CloseRequest);

        public void Init(string title, string description, Action onConfirm, Action onCancel)
        {
            _view.SetTitle(title);
            _view.SetDescription(description);
            
            _onConfirmCallback = onConfirm;
            _onCancelCallback = onCancel;
        }

        public override void Show()
        {
            base.Show();
            
            _view.ButtonConfirm.onClick.AddListener(OnConfirmClicked);
            _view.ButtonCancel.onClick.AddListener(OnCancelClicked);
        }

        public override void Hide()
        {
            base.Hide();
            
            _onConfirmCallback = null;
            _onCancelCallback = null;
            
            _view.ButtonConfirm.onClick.RemoveListener(OnConfirmClicked);
            _view.ButtonCancel.onClick.RemoveListener(OnCancelClicked);
        }
        
        public bool HandleKeyPressed(InputIntent inputIntent) =>
            _inputContext.HandleKeyPressed(inputIntent);

        private void OnConfirmClicked()
        {
            _onConfirmCallback?.Invoke();
            CloseRequest();
        }

        private void OnCancelClicked()
        {
            _onCancelCallback?.Invoke();
            CloseRequest();
        }
    }
}