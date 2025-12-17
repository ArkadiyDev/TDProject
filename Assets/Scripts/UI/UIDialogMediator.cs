namespace UI
{
    public abstract class UIDialogMediator<TView> : UIMediator<TView>, IUIDialogMediator where TView : IUiView
    {
        protected UIDialogMediator(TView view) : base(view)
        {
        }
    }
}