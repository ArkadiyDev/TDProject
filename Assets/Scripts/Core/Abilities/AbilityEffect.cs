using System;

namespace Core.Abilities
{
    [Serializable]
    public abstract class AbilityEffect : IAbilityEffect
    {
        public abstract void Apply(ICaster caster, ITarget target);
    }
}