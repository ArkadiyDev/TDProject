namespace UI
{
    public interface IUIService
    {
        void OpenWindow<TMediator>() where TMediator : IUiMediator;
        void OpenPopup<TMediator>() where TMediator : IUiPopupMediator;
        void OpenDialog<TMediator>() where TMediator : IUiDialogMediator;
        void HandleBackAction();
    }
}