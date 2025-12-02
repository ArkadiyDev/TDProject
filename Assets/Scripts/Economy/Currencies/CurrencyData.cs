using System;
using UnityEngine;

namespace Economy.Currencies
{
    [Serializable]
    public class CurrencyData
    {
        [SerializeField] private CurrencySettings _settings;
        [SerializeField] private int _amount;

        public CurrencySettings Settings => _settings;
        public int Amount => _amount;
    }
}