using System;
using Core.Invaders;

namespace Core.Abilities
{
    [Serializable]
    public class SelfAbilityTargeting : AbilityTargeting
    {
        public override void Start(AbilitySettings abilitySettings, TargetingProcessor targetingProcessor)
        {
            _abilitySettings = abilitySettings;
            _targetingProcessor = targetingProcessor;

            if (_targetingProcessor.TryGetComponent<InvaderLink>(out var invaderLink))
                abilitySettings.Execute(targetingProcessor, invaderLink.Damageable);
        }
    }
}