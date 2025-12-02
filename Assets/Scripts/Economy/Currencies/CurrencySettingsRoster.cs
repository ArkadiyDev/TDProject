using System.Collections.Generic;
using UnityEngine;

namespace Economy.Currencies
{
    [CreateAssetMenu(fileName = "NewCurrencySettingsRoster", menuName = "TD/Settings/" + nameof(CurrencySettingsRoster))]
    public class CurrencySettingsRoster : ScriptableObject
    {
        [SerializeField] private List<CurrencySettings> _currencies = new();

        public List<CurrencySettings> Currencies => _currencies;
    }
}