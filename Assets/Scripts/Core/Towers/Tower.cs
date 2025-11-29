using Core.Invaders;
using UnityEngine;

namespace Core.Towers
{
    public class Tower
    {
        private readonly TowerSettings _settings;
        private readonly TowerModel _model;
        private readonly TowerView _view;
        private readonly ProjectileViewPool _projectileViewPool;

        public Tower(TowerSettings settings, TowerModel model, TowerView view, ProjectileViewPool projectileViewPool)
        {
            _settings = settings;
            _model = model;
            _view = view;
            _projectileViewPool = projectileViewPool;
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

        private bool TryFindTarget(out InvaderView targetView)
        {
            return TargetFinderHelper.TryFindTarget(_view.transform.position, _model.Range, out targetView, _model.LayerMask);
        }

        private void UpdateFireTimer(float deltaTime)
        {
            _model.СurrentFireTimer += deltaTime;
        }

        private void Shoot(InvaderView targetView)
        {
            Debug.Log($"Shooting with damage {_model.Damage}");
            var projectiveView = _projectileViewPool.Get();
            projectiveView.transform.position = _view.ProjectileStartPoint.position;
            projectiveView.Launch(targetView.BodyTargetPoint, _settings.Projectile.Speed, () => _projectileViewPool.Release(projectiveView));
        }
    }
}