using System.Collections.Generic;
using UnityEngine;

namespace Core.Building
{
    public class BuildingProcessor : MonoBehaviour
    {
        private readonly List<IBuilding> _activeBuildings = new();

        public void RegisterTower(IBuilding building) =>
            _activeBuildings.Add(building);

        public void UnregisterTower(IBuilding building) =>
            _activeBuildings.Remove(building);

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var building in _activeBuildings)
                building.Tick(deltaTime);
        }
    }
}