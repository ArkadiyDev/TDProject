using System;

namespace UI
{
    public interface IUiMediator
    {
        event Action<IUiMediator> RequestClose;
        void Show();
        void Hide();
    }
}