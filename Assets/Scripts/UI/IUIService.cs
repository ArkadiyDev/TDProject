using InputSystem;

namespace UI
{
    public interface IUIService
    {
        TMediator OpenWindow<TMediator>() where TMediator : IUiMediator;
        TMediator OpenPopup<TMediator>() where TMediator : IUIPopupMediator;
        TMediator OpenDialog<TMediator>() where TMediator : IUIDialogMediator;
        void CloseCurrentWindow();
        void CloseCurrentDialog();
        void CloseCurrentPopup();
        bool ProcessKeyInput(InputIntent inputIntent);
    }
}