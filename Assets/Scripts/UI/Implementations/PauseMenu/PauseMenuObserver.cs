namespace UI.Implementations.PauseMenu
{
    public class PauseMenuObserver : UIObserver<PauseMenuMediator>
    {
        protected PauseMenuObserver(IUIService uiService, PauseMenuMediator mediator) : base(uiService, mediator)
        {
        }
        
        public override void Initialize()
        {
            _mediator.CloseRequested += OnCloseRequested;
        }

        private void OnCloseRequested()
        {
            _uiService.CloseCurrentWindow();
        }
    }
}