using UnityEngine;

namespace Core.Towers
{
    public class TowerFactory : ITowerFactory
    {
        private readonly TowerViewPool _towerViewPool;
        private readonly TowerSettings _towerSettings;
        private readonly TowerHandler _towerHandler;
        private readonly IProjectileFactory _projectileFactory;

        public TowerFactory(TowerSettings settings, TowerViewPool towerViewPool, TowerHandler towerHandler, IProjectileFactory projectileFactory)
        {
            _towerViewPool = towerViewPool;
            _towerSettings = settings;
            _towerHandler = towerHandler;
            _projectileFactory = projectileFactory;

            _towerViewPool.Init(_towerSettings.AssetReference);
        }

        public Tower CreateTower(Vector3 position)
        {
            var view = _towerViewPool.Get();
            var model = new TowerModel(_towerSettings);
            var tower = new Tower(_towerSettings, model, view, _projectileFactory);

            view.transform.position = position;
            view.gameObject.SetActive(true);
            view.InitializeVisuals(_towerSettings.Range);
            
            _towerHandler.RegisterTower(tower);

            return tower;
        }
    }
}