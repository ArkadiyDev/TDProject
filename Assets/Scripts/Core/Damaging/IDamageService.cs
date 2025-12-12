using System;

namespace Core.Damaging
{
    public interface IDamageService
    {
        event Action<IDamageable, float> OnDamageDealt;
        void ApplyDamage(IAttacker attacker, IDamageable target);
    }
}