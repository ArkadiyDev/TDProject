namespace Core.Invaders
{
    public class InvaderFactory
    {
        private readonly InvaderSettings _invaderSettings;
        private readonly InvaderViewPool _invaderViewPool;

        public InvaderFactory(InvaderSettings invaderSettings, InvaderViewPool invaderViewPool)
        {
            _invaderSettings = invaderSettings;
            _invaderViewPool = invaderViewPool;
            
            _invaderViewPool.Init(_invaderSettings.AssetReference);
        }

        public Invader Create()
        {
            var invaderView = _invaderViewPool.Get();

            return new Invader(_invaderSettings, invaderView, _invaderViewPool.Release);
        }
    }
}
