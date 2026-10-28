using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Abilities
{
    public class AbilityCaster : MonoBehaviour, IAbilityCaster
    {
        [SerializeField] private AbilitySettings[] _abilities;
        [SerializeField] private TargetingProcessor _targeting;

        public AbilitySettings[] Abilities => _abilities;
        
        public void Cast(AbilitySettings ability)
        {
            ability.Target(_targeting);
        }

        private void Update()
        {
            if (Keyboard.current.cKey.wasPressedThisFrame)
                Cast(_abilities[0]);
        }
    }
}