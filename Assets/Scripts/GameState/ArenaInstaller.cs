using Common.GameSpeed;
using Core.Arenas;
using Core.Building;
using Core.Castles;
using Core.Invaders;
using Core.Towers;
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
        [SerializeField] private TowerSettings _towerSettings;
        [SerializeField] private InvaderViewPool _invaderViewPool;
        [SerializeField] private TowerViewPool _towerViewPool;
        [SerializeField] private ProjectileViewPool _projectileViewPool;
        [SerializeField] private TowerHandler _towerHandler;
        [SerializeField] private BuildingPlacementService _buildingPlacementService;
        [SerializeField] private CoreInputService _inputService;

        public override void InstallBindings()
        {
            BindArenaSettings();
            BindCastleSettings();
            BindInvaderFactory();
            BindInputService();
            BindGameSpeedService();
            BindTowerFactory();
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
        
        private void BindTowerFactory()
        {
            Container
                .Bind<ITowerFactory>()
                .To<TowerFactory>()
                .AsSingle()
                .WithArguments(_towerSettings, _towerViewPool, _towerHandler, _projectileViewPool);
        }

        private void BindBuildingSystem()
        {
            var towerFactory = Container.Resolve<ITowerFactory>();
            
            Container
                .Bind<BuildingSystem>()
                .AsSingle()
                .WithArguments(_inputService, _buildingPlacementService, towerFactory)
                .NonLazy();
        }
    }
}
