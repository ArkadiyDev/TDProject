using System;

namespace Core.Damaging
{
    public class DamageService : IDamageService
    {
        public event Action<IDamageable, float> OnDamageDealt;
        
        public void ApplyDamage(IAttacker attacker, IDamageable target)
        {
            //TODO: make the damage calculation formula more complex
            var calculatedDamage = attacker.Damage;
            
            target.TakeDamage(calculatedDamage);
            
            OnDamageDealt?.Invoke(target, calculatedDamage); 
        }
    }
}