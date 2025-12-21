using System.Collections.Generic;
using Core.Arenas;
using Core.Towers;
using Economy.Wallets;
using UnityEngine;

namespace Core.Building
{
    public class BuildingService : IBuildingService, IResettable
    {
        private readonly IBuildingPlacementService _placementService;
        private readonly ITowerFactory _towerFactory;
        private readonly IWalletService _walletService;
        private readonly BuildingProcessor _buildingProcessor;
        private readonly List<IBuilding> _activeBuildings = new();

        public bool IsBuildingState => _placementService.IsBuildingState;

        public BuildingService(IBuildingPlacementService placementService, ITowerFactory towerFactory,
            IWalletService walletService, BuildingProcessor buildingProcessor)
        {
            _placementService = placementService;
            _towerFactory = towerFactory;
            _walletService = walletService;
            _buildingProcessor = buildingProcessor;

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
            
            _walletService.TryDecreaseCurrencies(towerCost);
        }

        public void RemoveBuilding(IBuilding building)
        {
            if (!_activeBuildings.Contains(building))
                return;
            
            _buildingProcessor.UnregisterTower(building);
            
            building.Demolish();
            _activeBuildings.Remove(building);
        }

        public void Reset() => RemoveAllBuildings();

        private void RemoveAllBuildings()
        {
            foreach (var building in _activeBuildings)
            {
                _buildingProcessor.UnregisterTower(building);
            
                building.Demolish();
            }
            
            _activeBuildings.Clear();
        }

        private void OnPlacementSuccessful(Vector3 position)
        {
            var tower = _towerFactory.CreateTower(position);
            
            _buildingProcessor.RegisterTower(tower);
            _activeBuildings.Add(tower);
        }
    }
}