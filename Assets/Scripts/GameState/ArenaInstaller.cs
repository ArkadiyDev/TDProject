using Common.GameSpeed;
using Core.Arenas;
using Core.Building;
using Core.Castles;
using Core.Invaders;
using Core.Towers;
using Economy;
using Economy.Currencies;
using Economy.Rewards;
using Economy.Wallets;
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
        [SerializeField] private CurrencySettingsRoster _currenciesRoster;
        [SerializeField] private InvaderViewPool _invaderViewPool;
        [SerializeField] private TowerViewPool _towerViewPool;
        [SerializeField] private ProjectileViewPool _projectileViewPool;
        [SerializeField] private TowerHandler _towerHandler;
        [SerializeField] private BuildingPlacementService _buildingPlacementService;
        [SerializeField] private CoreInputService _inputService;

        public override void InstallBindings()
        {
            BindWalletService();
            BindRewardProvider();
            BindArenaSettings();
            BindCastleSettings();
            BindInvaderFactory();
            BindInputService();
            BindGameSpeedService();
            BindProjectileFactory();
            BindTowerFactory();
            BindBuildingSystem();
        }

        private void BindWalletService()
        {
            Container
                .Bind<IWalletService>()
                .To<ArenaWalletService>()
                .AsSingle()
                .WithArguments(_currenciesRoster, _arenaSettings.StartCurrencies);
        }

        private void BindRewardProvider()
        {
            var walletService = Container.Resolve<IWalletService>();
            Container
                .Bind<IRewardProvider>()
                .To<ArenaRewardProvider>()
                .AsSingle()
                .WithArguments(walletService);
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
        
        private void BindProjectileFactory()
        {
            Container
                .Bind<IProjectileFactory>()
                .To<ProjectileFactory>()
                .AsSingle()
                .WithArguments(_projectileViewPool, _towerSettings.Projectile);
        }
        
        private void BindTowerFactory()
        {
            var projectileFactory =  Container.Resolve<IProjectileFactory>();
            
            Container
                .Bind<ITowerFactory>()
                .To<TowerFactory>()
                .AsSingle()
                .WithArguments(_towerSettings, _towerViewPool, _towerHandler, projectileFactory);
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
