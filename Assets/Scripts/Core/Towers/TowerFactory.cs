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
        private readonly TowerProcessor _towerProcessor;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IDamageService _damageService;

        public TowerFactory(TowerSettings settings, TowerViewPool towerViewPool, TowerProcessor towerProcessor,
            IProjectileFactory projectileFactory, IDamageService damageService)
        {
            _towerViewPool = towerViewPool;
            _towerSettings = settings;
            _towerProcessor = towerProcessor;
            _projectileFactory = projectileFactory;
            _damageService = damageService;

            _towerViewPool.Init(_towerSettings.AssetReference);
        }

        public Tower CreateTower(Vector3 position)
        {
            var view = _towerViewPool.Get();
            var model = new TowerModel(_towerSettings);
            var tower = new Tower(_towerSettings, model, view, _projectileFactory, _damageService);

            view.transform.position = position;
            view.gameObject.SetActive(true);
            view.InitializeVisuals(_towerSettings.Range);
            
            _towerProcessor.RegisterTower(tower);

            return tower;
        }

        public List<CurrencyData> GetTowerCost() =>
            _towerSettings.Cost;
    }
}