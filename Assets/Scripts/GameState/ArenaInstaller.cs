using Common.GameSpeed;
using Core.Arenas;
using Core.Castles;
using Core.Invaders;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class ArenaInstaller : MonoInstaller
    {
        [SerializeField] private ArenaSettings _arenaSettings;
        [SerializeField] private InvaderSettings _invaderSettings;
        [SerializeField] private CastleSettings _castleSettings;
        [SerializeField] private InvaderViewPool _invaderViewPool;

        public override void InstallBindings()
        {
            BindArenaSettings();
            BindCastleSettings();
            BindInvaderFactory();
            BindGameSpeedService();
        }

        private void BindArenaSettings()
        {
            Container
                .Bind<ArenaSettings>()
                .FromInstance(_arenaSettings)
                .AsSingle();
        }

        private void BindCastleSettings()
        {
            Container
                .Bind<CastleSettings>()
                .FromInstance(_castleSettings)
                .AsSingle();
        }

        private void BindInvaderFactory()
        {
            Container
                .Bind<InvaderFactory>()
                .AsSingle()
                .WithArguments(_invaderSettings, _invaderViewPool);
        }

        private void BindGameSpeedService()
        {
            Container
                .Bind<IGameSpeedService>()
                .To<GameSpeedService>()
                .AsSingle();
        }
    }
}
