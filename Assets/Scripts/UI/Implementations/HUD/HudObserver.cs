using UI.Implementations.ConfirmationDialog;
using UI.Implementations.PauseMenu;
using UI.Implementations.Popup;

namespace UI.Implementations.HUD
{
    public class HudObserver : UIObserver<HudMediator>
    {
        protected HudObserver(IUIService uiService, HudMediator mediator) : base(uiService, mediator)
        {
        }

        public override void Initialize()
        {
            _mediator.PauseMenuRequested += OnPauseMenuRequested;
            _mediator.PopupButtonPressed += OnPopupButtonPressed;
            _mediator.DialogButtonPressed += OnDialogButtonPressed;
        }

        public override void Dispose()
        {
            _mediator.PauseMenuRequested -= OnPauseMenuRequested;
            _mediator.PopupButtonPressed -= OnPopupButtonPressed;
            _mediator.DialogButtonPressed -= OnDialogButtonPressed;
        }

        private void OnPauseMenuRequested()
        {
            _uiService.OpenWindow<PauseMenuMediator>();
        }

        private void OnPopupButtonPressed()
        {
            _uiService.OpenPopup<PopupMediator>();
        }

        private void OnDialogButtonPressed()
        {
            _uiService.OpenDialog<ConfirmationDialogMediator>()
                .Init("Test dialog", "Press any button", null, null);
        }
    }
}