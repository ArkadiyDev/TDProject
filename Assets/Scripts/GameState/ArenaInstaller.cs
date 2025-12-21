using Common.GameSpeed;
using Common.UI.FloatingText;
using Core.Arenas;
using Core.Building;
using Core.Castles;
using Core.Damaging;
using Core.Invaders;
using Core.Towers;
using Economy.Currencies;
using Economy.Rewards;
using Economy.Wallets;
using InputSystem;
using UI;
using UI.Implementations.ConfirmationDialog;
using UI.Implementations.HUD;
using UI.Implementations.PauseMenu;
using UI.Implementations.Popup;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameState
{
    public class ArenaInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private ArenaSettings _arenaSettings;
        [SerializeField] private InvaderSettings _invaderSettings;
        [SerializeField] private CastleSettings _castleSettings;
        [SerializeField] private TowerSettings _towerSettings;
        [SerializeField] private CurrencySettingsRoster _currenciesRoster;
        [SerializeField] private FloatingTextPool _floatingTextPool;
        [SerializeField] private InvaderViewPool _invaderViewPool;
        [SerializeField] private TowerViewPool _towerViewPool;
        [SerializeField] private ProjectileViewPool _projectileViewPool;
        [SerializeField] private BuildingProcessor _buildingProcessor;
        [SerializeField] private InvaderSystem _invaderSystem;
        [SerializeField] private BuildingPlacementService _buildingPlacementService;
        [SerializeField] private CoreInputService _inputService;
        [SerializeField] private UICoreGameplayRoot _uiRoot;

        public override void InstallBindings()
        {
            BindInvaderProcessor();
            BindWalletService();
            BindRewardProvider();
            BindArenaSettings();
            BindCastleSettings();
            BindDamageService();
            BindFloatingTextService();
            BindInvaderDeathHandler();
            BindInvaderFactory();
            BindInputService();
            BindGameSpeedService();
            BindProjectileFactory();
            BindTowerFactory();
            BindBuildingService();
            BindArenaResetables();
            BindUIServices();
            BindObservers();
        }

        private void BindInvaderProcessor()
        {
            Container.BindInstance(_invaderSystem).AsSingle();
        }

        private void BindWalletService()
        {
            Container
                .BindInterfacesAndSelfTo<WalletService>()
                .AsSingle()
                .WithArguments(_currenciesRoster, _arenaSettings.StartCurrencies);
        }

        private void BindRewardProvider()
        {
            Container
                .Bind<IRewardProvider>()
                .To<ArenaRewardProvider>()
                .AsSingle();
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

        private void BindDamageService()
        {
            Container
                .Bind<IDamageService>()
                .To<DamageService>()
                .AsSingle();
        }

        private void BindFloatingTextService()
        {
            Container
                .Bind<IFloatingTextService>()
                .To<FloatingTextService>()
                .AsSingle()
                .WithArguments(_floatingTextPool, _camera);
        }

        private void BindInvaderDeathHandler()
        {
            Container
                .Bind<IInvaderDeathHandler>()
                .To<InvaderDeathHandler>()
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
            Container
                .Bind<IGameSpeedService>()
                .To<GameSpeedService>()
                .AsSingle()
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
            Container
                .Bind<ITowerFactory>()
                .To<TowerFactory>()
                .AsSingle()
                .WithArguments(_towerSettings, _towerViewPool);
        }

        private void BindBuildingService()
        {
            Container
                .BindInterfacesAndSelfTo<BuildingService>()
                .AsSingle()
                .WithArguments(_buildingPlacementService, _buildingProcessor)
                .NonLazy();
        }
        
        private void BindArenaResetables()
        {
            Container.Bind<ArenaRestartHandler>()
                .AsSingle();
        }

        private void BindUIServices()
        {
            Container.BindInstance(_uiRoot.HUDView).AsSingle();
            Container.BindInstance(_uiRoot.PauseMenuView).AsSingle();
            Container.BindInstance(_uiRoot.PopupView).AsSingle();
            Container.BindInstance(_uiRoot.ConfirmationDialogView).AsSingle();

            Container.BindInterfacesAndSelfTo<HudMediator>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseMenuMediator>().AsSingle();
            Container.BindInterfacesAndSelfTo<PopupMediator>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConfirmationDialogMediator>().AsSingle();
            
            Container.Bind<IUIService>().To<UIService>().AsSingle();

            Container.BindInterfacesTo<ConfirmationDialogObserver>().AsSingle();
            Container.BindInterfacesTo<HudObserver>().AsSingle();
            Container.BindInterfacesTo<PauseMenuObserver>().AsSingle();
            Container.BindInterfacesTo<PopupObserver>().AsSingle();
    
            Container.BindInterfacesAndSelfTo<UIInputMediator>().AsSingle();
        }

        private void BindObservers()
        {
            Container
                .Bind<IInitializable>()
                .To<DamageObserver>()
                .AsSingle();
        }
    }
}
