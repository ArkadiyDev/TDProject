namespace UI.Implementations.ConfirmationDialog
{
    public class ConfirmationDialogObserver : UIObserver<ConfirmationDialogMediator>
    {
        protected ConfirmationDialogObserver(IUIService uiService, ConfirmationDialogMediator mediator) : base(uiService, mediator)
        {
        }
        
        public override void Initialize()
        {
            _mediator.CloseRequested += OnCloseRequested;
        }

        private void OnCloseRequested()
        {
            _uiService.CloseCurrentDialog();
        }
    }
}