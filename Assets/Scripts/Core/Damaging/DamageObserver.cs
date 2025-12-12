using System.Globalization;
using Common.UI.FloatingText;
using Zenject;

namespace Core.Damaging
{
    public class DamageObserver : IInitializable
    {
        private readonly IDamageService _damageService;
        private readonly IFloatingTextService _floatingTextService;

        public DamageObserver(IDamageService damageService, IFloatingTextService floatingTextService)
        {
            _damageService = damageService;
            _floatingTextService = floatingTextService;
        }

        public void Initialize() =>
            _damageService.OnDamageDealt += OnDamageDealtHandler;

        private void OnDamageDealtHandler(IDamageable target, float amount) =>
            _floatingTextService.ShowText(target.BodyPoint.position, amount.ToString(CultureInfo.InvariantCulture));
    }
}