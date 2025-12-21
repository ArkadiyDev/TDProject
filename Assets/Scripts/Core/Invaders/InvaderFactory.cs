using Core.Arenas;

namespace Core.Invaders
{
    public class InvaderFactory
    {
        private readonly InvaderSettings _invaderSettings;
        private readonly InvaderViewPool _invaderViewPool;
        private readonly IInvaderDeathHandler _invaderDeathHandler; 
        private readonly InvaderSystem _invaderSystem; 

        public InvaderFactory(InvaderSettings invaderSettings, InvaderViewPool invaderViewPool,
            IInvaderDeathHandler invaderDeathHandler, InvaderSystem invaderSystem)
        {
            _invaderSettings = invaderSettings;
            _invaderViewPool = invaderViewPool;
            _invaderDeathHandler = invaderDeathHandler;
            _invaderSystem = invaderSystem;
            
            _invaderViewPool.Init(_invaderSettings.AssetReference);
        }

        public Invader Create(Route route)
        {
            var view = _invaderViewPool.Get();
            var invader = new Invader(_invaderSettings, view);
        
            invader.SetStartPosition(route.Spawner.Position);
            invader.StartRoute(route);
            invader.SetActiveView(true);

            invader.Died += i => _invaderDeathHandler.InvaderDeathHandle(i);
            _invaderSystem.Add(invader);
            invader.Removed += (i) => _invaderViewPool.Release(i.View);

            return invader;
        }
    }
}
