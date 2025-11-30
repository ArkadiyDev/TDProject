using UnityEngine;

namespace Core.Towers
{
    public interface IProjectileFactory
    {
        ProjectileView CreateProjectile(Vector3 startPos);
    }
}