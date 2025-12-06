using UnityEngine;

namespace Core.Invaders
{
    public interface IDamageable
    {
        void TakeDamage(float damageAmount);
        bool IsDead { get; }
        Transform BodyPoint { get; }
    }
}