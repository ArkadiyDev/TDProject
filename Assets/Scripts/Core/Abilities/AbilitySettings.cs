using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "NewAbilitySettings", menuName = "TD/Settings/" + nameof(AbilitySettings))]
    public class AbilitySettings : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        private List<IAbilityEffect> _effects = new();

        [SerializeReference, SubclassSelector]
        private AbilityTargeting _targeting;

        public void Target(TargetingProcessor targetingProcessor) =>
            _targeting?.Start(this, targetingProcessor);

        public void Execute(ICaster caster, ITarget target)
        {
            foreach (var effect in _effects)
                effect.Apply(caster, target);
        }
    }
}