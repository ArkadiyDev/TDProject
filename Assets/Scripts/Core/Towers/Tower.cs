using Core.Damaging;
using UnityEngine;

namespace Core.Towers
{
    public class Tower : IAttacker
    {
        private readonly TowerSettings _settings;
        private readonly TowerModel _model;
        private readonly TowerView _view;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IDamageService _damageService;

        public float Damage => _model.Damage;

        public Tower(TowerSettings settings, TowerModel model, TowerView view, IProjectileFactory projectileFactory,
            IDamageService damageService)
        {
            _settings = settings;
            _model = model;
            _view = view;
            _projectileFactory = projectileFactory;
            _damageService = damageService;
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

        private bool TryFindTarget(out IDamageable target) =>
            _settings.TargetsFinder.TryFindTarget(_view.transform.position, _model.Range, out target);

        private void UpdateFireTimer(float deltaTime) =>
            _model.СurrentFireTimer += deltaTime;

        private void Shoot(IDamageable target)
        {
            Debug.Log($"Shooting with damage {_model.Damage}");
            var projectile = _projectileFactory.CreateProjectile(_view.ProjectileStartPoint.position);
            
            projectile.Launch(target.BodyPoint, _settings.Projectile.Speed,
                () => _damageService.ApplyDamage(this, target));
        }
    }
}