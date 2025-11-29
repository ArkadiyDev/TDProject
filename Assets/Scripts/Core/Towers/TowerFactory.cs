using UnityEngine;

namespace Core.Towers
{
    public class TowerFactory : ITowerFactory
    {
        private readonly TowerViewPool _towerViewPool;
        private readonly TowerSettings _towerSettings;
        private readonly TowerHandler _towerHandler;
        private readonly ProjectileViewPool _projectileViewPool;

        public TowerFactory(TowerSettings settings, TowerViewPool towerViewPool, TowerHandler towerHandler, ProjectileViewPool projectileViewPool)
        {
            _towerViewPool = towerViewPool;
            _towerSettings = settings;
            _towerHandler = towerHandler;
            _projectileViewPool = projectileViewPool;

            _towerViewPool.Init(_towerSettings.AssetReference);
            _projectileViewPool.Init(_towerSettings.Projectile.AssetReference);
        }

        public Tower CreateTower(Vector3 position)
        {
            var view = _towerViewPool.Get();
            var model = new TowerModel(_towerSettings);
            var tower = new Tower(_towerSettings, model, view, _projectileViewPool);

            view.transform.position = position;
            view.InitializeVisuals(_towerSettings.Range);
            
            _towerHandler.RegisterTower(tower);

            return tower;
        }
    }
}