using System;

namespace UI
{
    public abstract class UIMediator<TView> : IUiMediator where TView : IUiView
    {
        public event Action CloseRequested;

        protected readonly TView _view;
        public virtual void Show() => _view.Open();
        public virtual void Hide() => _view.Close();

        protected UIMediator(TView view) =>
            _view = view;

        protected void CloseRequest() =>
            CloseRequested?.Invoke();
    }
}