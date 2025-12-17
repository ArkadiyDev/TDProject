namespace UI
{
    public abstract class UIPopupMediator<TView> : UIMediator<TView>, IUIPopupMediator where TView : IUiView
    {
        protected UIPopupMediator(TView view) : base(view)
        {
        }
    }
}