namespace Core.Invaders
{
    public class InvaderFactory
    {
        private readonly InvaderSettings _invaderSettings;
        private readonly InvaderViewPool _invaderViewPool;
        private readonly IInvaderDeathHandler _invaderDeathHandler; 

        public InvaderFactory(InvaderSettings invaderSettings, InvaderViewPool invaderViewPool, IInvaderDeathHandler invaderDeathHandler)
        {
            _invaderSettings = invaderSettings;
            _invaderViewPool = invaderViewPool;
            _invaderDeathHandler = invaderDeathHandler;
            
            _invaderViewPool.Init(_invaderSettings.AssetReference);
        }

        public Invader Create()
        {
            var invaderView = _invaderViewPool.Get();

            var invader = new Invader(_invaderSettings, invaderView);

            invader.Removed += OnInvaderRemoved;
            invader.Died += OnInvaderDied;

            return invader;
        }

        private void OnInvaderRemoved(Invader invader) =>
            _invaderViewPool.Release(invader.View);

        private void OnInvaderDied(InvaderSettings invaderSettings) =>
            _invaderDeathHandler.InvaderDeathHandle(invaderSettings);
    }
}
