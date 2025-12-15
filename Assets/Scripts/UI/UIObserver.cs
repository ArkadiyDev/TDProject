using Zenject;

namespace UI
{
    public abstract class UIObserver<TMediator> : IInitializable where TMediator : IUiMediator
    {
        protected readonly IUIService _uiService;
        protected TMediator _mediator;

        protected UIObserver(IUIService uiService, TMediator mediator)
        {
            _uiService = uiService;
            _mediator = mediator;
        }

        public virtual void Initialize()
        {
        }
    }
}