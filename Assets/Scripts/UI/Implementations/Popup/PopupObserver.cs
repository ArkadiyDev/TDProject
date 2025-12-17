using UI.Implementations.ConfirmationDialog;

namespace UI.Implementations.Popup
{
    public class PopupObserver : UIObserver<PopupMediator>
    {
        protected PopupObserver(IUIService uiService, PopupMediator mediator) : base(uiService, mediator)
        {
        }
        
        public override void Initialize()
        {
            _mediator.CloseRequested += OnCloseRequested;
            _mediator.DialogButtonPressed += OnDialogButtonPressed;
        }

        private void OnCloseRequested()
        {
            _uiService.CloseCurrentPopup();
        }
        
        private void OnDialogButtonPressed()
        {
            _uiService.OpenDialog<ConfirmationDialogMediator>()
                .Init("Test dialog above popup", "Press any button", null, null);
        }
    }
}