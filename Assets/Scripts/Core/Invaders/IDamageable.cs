using UnityEngine;

namespace Core.Invaders
{
    public interface IDamageable
    {
        void TakeDamage(float damageAmount);
        bool IsAlive { get; }
        Transform BodyPoint { get; }
    }
}