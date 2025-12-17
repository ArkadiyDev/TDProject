using System;

namespace UI
{
    public interface IUiMediator
    {
        event Action CloseRequested;
        void Show();
        void Hide();
    }
}