using System;
using System.Collections.Generic;
using Economy.Currencies;
using UnityEngine;

namespace Economy.Rewards
{
    [Serializable]
    public class RewardData
    {
        [SerializeField] List<CurrencyData> _currencies;
        
        public List<CurrencyData> Currencies => _currencies;
    }
}