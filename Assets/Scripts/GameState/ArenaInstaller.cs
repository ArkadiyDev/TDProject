using Common.GameSpeed;
using Core.Arenas;
using Core.Building;
using Core.Castles;
using Core.Invaders;
using InputSystem;
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
        [SerializeField] private BuildingViewPool _buildingViewPool;
        [SerializeField] private BuildingPlacementService _buildingPlacementService;
        [SerializeField] private CoreInputService _inputService;

        public override void InstallBindings()
        {
            BindArenaSettings();
            BindCastleSettings();
            BindInvaderFactory();
            BindInputService();
            BindGameSpeedService();
            BindBuildingSystem();
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

        private void BindInputService()
        {
            Container
                .Bind<IInputService>()
                .FromInstance(_inputService)
                .AsSingle();
        }

        private void BindGameSpeedService()
        {
            var inputService = Container.Resolve<IInputService>();

            Container
                .Bind<IGameSpeedService>()
                .To<GameSpeedService>()
                .AsSingle()
                .WithArguments(inputService)
                .NonLazy();
        }

        private void BindBuildingSystem()
        {
            Container
                .Bind<BuildingSystem>()
                .AsSingle()
                .WithArguments(_inputService, _buildingPlacementService, _buildingViewPool)
                .NonLazy();
        }
    }
}
