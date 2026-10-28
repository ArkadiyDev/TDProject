using Core.Damaging;

namespace Core.Abilities
{
    public class AbilityServices
    {
        public IDamageService DamageService { get; }
    
        public AbilityServices(IDamageService damageService)
        {
            DamageService = damageService;
        }
    }
}