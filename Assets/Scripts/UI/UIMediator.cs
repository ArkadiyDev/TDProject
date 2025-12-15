using System;

namespace UI
{
    public abstract class UIMediator<TView> : IUiMediator where TView : IUiView
    {
        public event Action<IUiMediator> RequestClose;

        protected readonly TView _view;
        public virtual void Show() => _view.Open();
        public virtual void Hide() => _view.Close();

        protected UIMediator(TView view) =>
            _view = view;

        protected void OnCloseButtonPressed() =>
            RequestClose?.Invoke(this);
    }
}