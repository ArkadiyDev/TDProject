using UnityEngine;

namespace Core.Towers
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ProjectileViewPool _projectileViewPool;

        public ProjectileFactory(ProjectileViewPool projectileViewPool, ProjectileSettings projectileSettings)
        {
            _projectileViewPool = projectileViewPool;
            _projectileViewPool.Init(projectileSettings.AssetReference);
        }

        public ProjectileView CreateProjectile(Vector3 startPos)
        {
            var projectile = _projectileViewPool.Get();
            projectile.transform.position = startPos;
            projectile.gameObject.SetActive(true);
            projectile.SetOnCompletionAction(() => _projectileViewPool.Release(projectile));
            
            return projectile;
        }
    }
}