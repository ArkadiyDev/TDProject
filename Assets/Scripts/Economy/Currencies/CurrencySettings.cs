using UnityEngine;

namespace Economy.Currencies
{
    [CreateAssetMenu(fileName = "NewCurrencySettings", menuName = "TD/Settings/" + nameof(CurrencySettings))]
    public class CurrencySettings : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public string Name => _name;
        public Sprite Icon => _icon;
    }
}