namespace Core.Invaders
{
    public class InvaderFactory
    {
        private readonly InvaderSettings _invaderSettings;
        private readonly InvaderViewPool _invaderViewPool;
        private readonly IInvaderDeathHandler _invaderDeathHandler; 
        private readonly InvaderProcessor _invaderProcessor; 

        public InvaderFactory(InvaderSettings invaderSettings, InvaderViewPool invaderViewPool,
            IInvaderDeathHandler invaderDeathHandler, InvaderProcessor invaderProcessor)
        {
            _invaderSettings = invaderSettings;
            _invaderViewPool = invaderViewPool;
            _invaderDeathHandler = invaderDeathHandler;
            _invaderProcessor = invaderProcessor;
            
            _invaderViewPool.Init(_invaderSettings.AssetReference);
        }

        public Invader Create()
        {
            var invaderView = _invaderViewPool.Get();

            var invader = new Invader(_invaderSettings, invaderView);

            invader.Removed += OnInvaderRemoved;
            invader.Died += OnInvaderDied;
            
            _invaderProcessor.RegisterInvader(invader);

            return invader;
        }

        private void OnInvaderRemoved(Invader invader)
        {
            _invaderProcessor.UnregisterInvader(invader);
            _invaderViewPool.Release(invader.View);
        }

        private void OnInvaderDied(Invader invader) =>
            _invaderDeathHandler.InvaderDeathHandle(invader);
    }
}
