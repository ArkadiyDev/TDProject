using InputSystem;
using UnityEngine;

namespace Core.Building
{
    public class BuildingSystem
    {
        private readonly IInputService _inputService;
        private readonly IBuildingPlacementService _placementService;
        private readonly BuildingViewPool _buildingViewPool;

        private BuildingSettings _buildingSettings;

        public BuildingSystem(IInputService inputService, IBuildingPlacementService placementService, BuildingViewPool buildingViewPool)
        {
            _inputService = inputService;
            _placementService = placementService;
            _buildingViewPool = buildingViewPool;

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
            var view = _buildingViewPool.Get();
            view.transform.position = position;
        }
    }
}