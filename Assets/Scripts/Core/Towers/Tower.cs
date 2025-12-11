using Core.Invaders;
using UnityEngine;

namespace Core.Towers
{
    public class Tower
    {
        private readonly TowerSettings _settings;
        private readonly TowerModel _model;
        private readonly TowerView _view;
        private readonly IProjectileFactory _projectileFactory;

        public Tower(TowerSettings settings, TowerModel model, TowerView view, IProjectileFactory projectileFactory)
        {
            _settings = settings;
            _model = model;
            _view = view;
            _projectileFactory = projectileFactory;
        }

        public void Tick(float deltaTime)
        {
            UpdateFireTimer(deltaTime);

            if (!_model.CanShoot())
                return;
            
            if(!TryFindTarget(out var target))
                return;
            
            Shoot(target);
            _model.ResetFireTimer();
        }

        private bool TryFindTarget(out IDamageable target)
        {
            return _settings.TargetsFinder.TryFindTarget(_view.transform.position, _model.Range, out target);
        }

        private void UpdateFireTimer(float deltaTime)
        {
            _model.СurrentFireTimer += deltaTime;
        }

        private void Shoot(IDamageable target)
        {
            Debug.Log($"Shooting with damage {_model.Damage}");
            var projectile = _projectileFactory.CreateProjectile(_view.ProjectileStartPoint.position);
            
            projectile.Launch(target, _settings.Projectile.Speed, _settings.Damage);
        }
    }
}