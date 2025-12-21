using System.Collections.Generic;
using Core.Damaging;
using Economy.Currencies;
using UnityEngine;

namespace Core.Towers
{
    public class TowerFactory : ITowerFactory
    {
        private readonly TowerViewPool _towerViewPool;
        private readonly TowerSettings _towerSettings;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IDamageService _damageService;

        public TowerFactory(TowerSettings settings, TowerViewPool towerViewPool, IProjectileFactory projectileFactory,
            IDamageService damageService)
        {
            _towerViewPool = towerViewPool;
            _towerSettings = settings;
            _projectileFactory = projectileFactory;
            _damageService = damageService;

            _towerViewPool.Init(_towerSettings.AssetReference);
        }

        public Tower CreateTower(Vector3 position)
        {
            var view = _towerViewPool.Get();
            var tower = new Tower(_towerSettings, view, _projectileFactory, _damageService);

            view.transform.position = position;
            view.gameObject.SetActive(true);
            view.InitializeVisuals(_towerSettings.Range);

            tower.Demolished += () => _towerViewPool.Release(view);
            
            return tower;
        }

        public List<CurrencyData> GetTowerCost() =>
            _towerSettings.Cost;
    }
}