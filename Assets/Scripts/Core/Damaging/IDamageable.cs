using Core.Abilities;

namespace Core.Damaging
{
    public interface IDamageable : ITarget
    {
        void TakeDamage(float damageAmount);
    }
}