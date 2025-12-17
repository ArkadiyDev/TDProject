using UI.Implementations.ConfirmationDialog;
using UI.Implementations.HUD;
using UI.Implementations.PauseMenu;
using UI.Implementations.Popup;
using UnityEngine;

namespace UI
{
    public class UICoreGameplayRoot : MonoBehaviour
    {
        [SerializeField] private HudView _hudView;
        [SerializeField] private PauseMenuView _pauseMenuView;
        [SerializeField] private PopupView _popupView;
        [SerializeField] private ConfirmationDialogView _confirmationDialogView;

        public HudView HUDView => _hudView;
        public PauseMenuView PauseMenuView => _pauseMenuView;
        public PopupView PopupView => _popupView;
        public ConfirmationDialogView ConfirmationDialogView => _confirmationDialogView;
    }
}