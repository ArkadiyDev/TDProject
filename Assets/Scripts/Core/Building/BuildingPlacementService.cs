using System;
using InputSystem;
using UnityEngine;

namespace Core.Building
{
    public class BuildingPlacementService : MonoBehaviour, IBuildingPlacementService
    {
        public event Action<Vector3> OnPlacementSuccessful;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private BuildingGhostView _ghost;
        [SerializeField] private LayerMask _buildableLayer;
        [SerializeField] private LayerMask _ignoreLayers;
        [SerializeField] private float _gridSize = 0.5f;

        [SerializeField] private BuildingSettings _buildingSettings;

        private IInputService _inputService;
        
        private bool _isBuildingState;
        private Vector3 _lastGhostPosition;

        public bool IsBuildingState => _isBuildingState;

        private void Update()
        {
            if (!_isBuildingState)
                return;

            MoveGhost(true);
        }

        public void StartPlacement()
        {
            ChangeBuildingState(true);
        }

        public void StopPlacement()
        {
            ChangeBuildingState(false);
        }

        public bool TryPlaceBuilding()
        {
            if (!_isBuildingState)
                return false;
            
            if (CanBuild())
            {
                OnPlacementSuccessful?.Invoke(_ghost.transform.position);
                _isBuildingState = false;
                _ghost.SetActive(false);
                
                return true;
            }

            Debug.Log("Impossible to build on this place!");
            return false;
        }

        private void ChangeBuildingState(bool isBuilding)
        {
            _isBuildingState = isBuilding;
            _ghost.SetActive(isBuilding);
            
            if (isBuilding)
                MoveGhost(true);
        }

        private void MoveGhost(bool forceUpdate = false)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if(!Physics.Raycast(ray, out var hit, float.MaxValue, ~_ignoreLayers))
                return;

            var projectedPosition = hit.point;
            projectedPosition.x = Mathf.Round(projectedPosition.x / _gridSize) * _gridSize;
            projectedPosition.z = Mathf.Round(projectedPosition.z / _gridSize) * _gridSize;
            
            _ghost.SetPosition(projectedPosition);
            
            if (!forceUpdate && !(Vector3.Distance(projectedPosition, _lastGhostPosition) > Mathf.Epsilon))
                return;
            
            _ghost.SetPosition(projectedPosition);
            _lastGhostPosition = projectedPosition;
            
            _ghost.SetAvailable(CanBuild());
        }

        private bool CanBuild()
        {
            if (!_ghost.OnBuildableSite)
                return false;

            if (!_ghost.SpaceIsFree)
                return false;
            
            if (!CornersOnBuildableSite())
                return false;

            return true;
        }

        private bool CornersOnBuildableSite()
        {
            var corners = GetBuildingCorners();
            foreach (var corner in corners)
            {
                if (!Physics.Raycast(corner + Vector3.up, Vector3.down, out _, 1.0f, _buildableLayer))
                    return false;
            }

            return true;
        }

        private Vector3[] GetBuildingCorners()
        {
            var corners = new Vector3[4];
            var position = _ghost.transform.position;
            var size = _buildingSettings.Size;
            
            corners[0] = new Vector3(position.x - size.x / 2, position.y, position.z - size.y / 2);
            corners[1] = new Vector3(position.x + size.x / 2, position.y, position.z - size.y / 2);
            corners[2] = new Vector3(position.x - size.x / 2, position.y, position.z + size.y / 2);
            corners[3] = new Vector3(position.x + size.x / 2, position.y, position.z + size.y / 2);

            return corners;
        }
    }
}