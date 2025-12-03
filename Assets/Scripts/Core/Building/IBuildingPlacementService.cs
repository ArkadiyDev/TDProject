using System;
using UnityEngine;

namespace Core.Building
{
    public interface IBuildingPlacementService
    {
        event Action<Vector3> OnPlacementSuccessful;
        bool IsBuildingState { get; }
        void StartPlacement();
        void StopPlacement();
        bool TryPlaceBuilding();
    }
}