using UnityEngine;

namespace Core.Damaging
{
    public interface IDamageable
    {
        void TakeDamage(float damageAmount);
        bool IsDead { get; }
        Transform BodyPoint { get; }
    }
}