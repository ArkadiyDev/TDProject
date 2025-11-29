using System.Collections.Generic;
using UnityEngine;

namespace Core.Towers
{
    public class TowerHandler : MonoBehaviour
    {
        private readonly List<Tower> _activeTowers = new();

        public void RegisterTower(Tower tower)
        {
            _activeTowers.Add(tower);
        }

        public void UnregisterTower(Tower tower)
        {
            _activeTowers.Remove(tower);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var tower in _activeTowers)
                tower.Tick(deltaTime);
        }
    }
}