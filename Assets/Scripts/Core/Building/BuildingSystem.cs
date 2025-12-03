using Core.Towers;
using Economy.Wallets;
using InputSystem;
using UnityEngine;

namespace Core.Building
{
    public class BuildingSystem
    {
        private readonly IInputService _inputService;
        private readonly IBuildingPlacementService _placementService;
        private readonly ITowerFactory _towerFactory;
        private readonly IWalletService _walletService;

        public BuildingSystem(IInputService inputService, IBuildingPlacementService placementService,
            ITowerFactory towerFactory, IWalletService walletService)
        {
            _inputService = inputService;
            _placementService = placementService;
            _towerFactory = towerFactory;
            _walletService = walletService;

            _inputService.OnBuildingClicked += OnBuildingClicked;
            _inputService.OnLeftMouseButtonClicked += OnLeftMouseButtonClicked;
            _placementService.OnPlacementSuccessful += OnPlacementSuccessful;
        }

        private void OnBuildingClicked()
        {
            if(_placementService.IsBuildingState)
                _placementService.StopPlacement();
            else
                _placementService.StartPlacement();
        }

        private void OnLeftMouseButtonClicked()
        {
            TryBuildTower();
        }

        private void OnPlacementSuccessful(Vector3 position)
        {
            _towerFactory.CreateTower(position);
        }

        private bool TryBuildTower()
        {
            var towerCost = _towerFactory.GetTowerCost();

            if (!_walletService.TryDecreaseCurrencies(towerCost))
                return false;

            if (!_placementService.TryPlaceBuilding())
                return false;
            
            _walletService.TryDecreaseCurrencies(towerCost);
            return true;
        }
    }
}