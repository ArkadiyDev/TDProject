using Core.Towers;
using InputSystem;
using UnityEngine;

namespace Core.Building
{
    public class BuildingSystem
    {
        private readonly IInputService _inputService;
        private readonly IBuildingPlacementService _placementService;
        private readonly ITowerFactory _towerFactory;

        public BuildingSystem(IInputService inputService, IBuildingPlacementService placementService, ITowerFactory towerFactory)
        {
            _inputService = inputService;
            _placementService = placementService;
            _towerFactory = towerFactory;

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
            _placementService.TryPlaceBuilding();
        }

        private void OnPlacementSuccessful(Vector3 position)
        {
            _towerFactory.CreateTower(position);
        }
    }
}