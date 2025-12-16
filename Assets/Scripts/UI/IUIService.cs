using InputSystem;

namespace UI
{
    public interface IUIService
    {
        void OpenWindow<TMediator>() where TMediator : IUiMediator;
        void OpenPopup<TMediator>() where TMediator : IUIPopupMediator;
        void OpenDialog<TMediator>() where TMediator : IUIDialogMediator;
        void CloseCurrentWindow();
        bool ProcessKeyInput(InputIntent inputIntent);
    }
}