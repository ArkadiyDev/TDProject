using System;
using Common.GameSpeed;
using Core.Building;
using Economy.Wallets;
using InputSystem;

namespace UI.Implementations.HUD
{
    public class HudMediator : UIMediator<HudView>, IInputHandler
    {
        private const string GoldId = "gold";
        
        public event Action PauseMenuRequested;
        public event Action PopupButtonPressed;
        public event Action DialogButtonPressed;
        
        private readonly IWalletService _walletService;
        private readonly HudInputContext _inputContext;

        public HudMediator(HudView view, IWalletService walletService, IGameSpeedService gameSpeedService,
            IBuildingService buildingService) : base(view)
        {
            _walletService = walletService;
            _inputContext = new HudInputContext(gameSpeedService, buildingService, PauseMenuRequest);
        }
        
        public override void Show()
        {
            base.Show();
            _walletService.CurrencyChanged += OnMoneyChanged;
            _view.SetGoldCounter(_walletService.GetCurrency(GoldId));

            _view.PopupButton.onClick.AddListener(PopupButtonPress);
            _view.DialogButton.onClick.AddListener(DialogButtonPress);
        }

        public override void Hide()
        {
            base.Hide();
            _walletService.CurrencyChanged -= OnMoneyChanged;
            
            _view.PopupButton.onClick.RemoveListener(PopupButtonPress);
            _view.DialogButton.onClick.RemoveListener(DialogButtonPress);
        }

        public bool HandleKeyPressed(InputIntent inputIntent) =>
            _inputContext.HandleKeyPressed(inputIntent);

        private void OnMoneyChanged(string id, int delta)
        {
            if(id != GoldId)
                return;
            
            _view.SetGoldCounter(_walletService.GetCurrency(GoldId));
        }

        private void PopupButtonPress() =>
            PopupButtonPressed?.Invoke();

        private void DialogButtonPress() =>
            DialogButtonPressed?.Invoke();

        private void PauseMenuRequest() =>
            PauseMenuRequested?.Invoke();
    }
}