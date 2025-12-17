using Common.GameSpeed;
using InputSystem;

namespace UI.Implementations.PauseMenu
{
    public class PauseMenuMediator : UIMediator<PauseMenuView>, IInputHandler
    {
        private readonly IGameSpeedService _speedService;
        private readonly PauseMenuInputContext _inputContext;

        public PauseMenuMediator(PauseMenuView view, IGameSpeedService speedService) : base(view)
        {
            _speedService = speedService;
            _inputContext = new PauseMenuInputContext(CloseRequest);
        }

        public override void Show()
        {
            base.Show();
            _speedService.SetPaused();
        }

        public override void Hide()
        {
            base.Hide();
            _speedService.SetUnpaused();
        }

        public bool HandleKeyPressed(InputIntent inputIntent) =>
            _inputContext.HandleKeyPressed(inputIntent);
    }
}