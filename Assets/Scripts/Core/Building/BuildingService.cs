using Core.Towers;
using Economy.Wallets;
using UnityEngine;

namespace Core.Building
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingPlacementService _placementService;
        private readonly ITowerFactory _towerFactory;
        private readonly IWalletService _walletService;
        
        public bool IsBuildingState => _placementService.IsBuildingState;

        public BuildingService(IBuildingPlacementService placementService, ITowerFactory towerFactory, IWalletService walletService)
        {
            _placementService = placementService;
            _towerFactory = towerFactory;
            _walletService = walletService;

            _placementService.OnPlacementSuccessful += OnPlacementSuccessful;
        }

        public void SwitchBuildingMode()
        {
            if(IsBuildingState)
                _placementService.StopPlacement();
            else
                _placementService.StartPlacement();
        }
        
        public void TryBuildTower()
        {
            if(!IsBuildingState)
                return;
            
            var towerCost = _towerFactory.GetTowerCost();
            
            if(!_walletService.CanAfford(towerCost))
                return;

            if (!_placementService.TryPlaceBuilding())
                return;
            
            if (!_walletService.TryDecreaseCurrencies(towerCost))
                return;
            
            _walletService.TryDecreaseCurrencies(towerCost);
        }

        private void OnPlacementSuccessful(Vector3 position) =>
            _towerFactory.CreateTower(position);
    }
}