using System;
using Core.Damaging;
using UnityEngine;

namespace Core.Abilities
{
    [AddTypeMenu("Ability/DamageEffect")]
    [Serializable]
    public class DamageEffectSettings : AbilityEffect
    {
        [SerializeField] private float _value;

        public override void Apply(ICaster caster, ITarget target)
        {
            Debug.Log($"{caster} take {_value} damage to {target}");
            
            if(target is IDamageable damageable)
                damageable.TakeDamage(_value);
        }
    }
}