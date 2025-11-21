using Core.Invaders;
using Zenject;

namespace Core.Arenas
{
    public class Arena
    {
        private readonly ArenaView _arenaView;
        private readonly ArenaModel _arenaModel;

        public Arena(DiContainer container)
        {
            var arenaSettings = container.Resolve<ArenaSettings>();
            var invaderFactory = container.Resolve<InvaderFactory>();
            
            _arenaView = container.Resolve<ArenaView>();
            _arenaModel = new ArenaModel(arenaSettings, invaderFactory, _arenaView.Spawners);

            _arenaModel.StartNextWave();
        }
    }
}
