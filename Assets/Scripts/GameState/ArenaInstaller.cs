using Core.Arenas;
using Core.Invaders;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class ArenaInstaller : MonoInstaller
    {
        [SerializeField] private ArenaSettings _arenaSettings;
        [SerializeField] private InvaderSettings _invaderSettings;
        [SerializeField] private ArenaView _arenaView;
        [SerializeField] private InvaderViewPool _invaderViewPool;

        public override void InstallBindings()
        {
            BindArenaSettings();
            BindArenaView();
            BindArena();
            BindInvaderFactory();
        }

        private void BindArenaSettings()
        {
            Container
                .Bind<ArenaSettings>()
                .FromInstance(_arenaSettings)
                .AsSingle();
        }

        private void BindArenaView()
        {
            Container
                .Bind<ArenaView>()
                .FromInstance(_arenaView)
                .AsSingle();
        }

        private void BindArena()
        {
            Container
                .Bind<Arena>()
                .AsSingle()
                .NonLazy();
        }

        private void BindInvaderFactory()
        {
            Container
                .Bind<InvaderFactory>()
                .AsSingle()
                .WithArguments(_invaderSettings, _invaderViewPool);
        }
    }
}
