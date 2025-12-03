using System;
using System.Collections.Generic;
using System.Linq;
using Economy.Currencies;
using UnityEngine;

namespace Economy.Wallets
{
    public class WalletService : IWalletService
    {
        public event Action<string, int> CurrencyChanged;

        private readonly Dictionary<string, int> _currencies = new();
        
        public WalletService(CurrencySettingsRoster currencySettingsRoster, List<CurrencyData> startCurrencies)
        {
            foreach (var settings in currencySettingsRoster.Currencies)
                _currencies.Add(settings.Id, 0);

            foreach (var startCurrency in startCurrencies)
                ChangeCurrency(startCurrency.Settings.Id, startCurrency.Amount);
        }

        public int GetCurrency(string currencyId)
        {
            if(!_currencies.TryGetValue(currencyId, out var value))
                throw new Exception($"Currency {currencyId} not found!");
            
            return value;
        }

        public void IncreaseCurrencies(List<CurrencyData> currencies)
        {
            foreach (var currency in currencies)
                IncreaseCurrency(currency.Settings.Id, currency.Amount);
        }

        public bool TryDecreaseCurrencies(List<CurrencyData> currencies)
        {
            if (!CanAfford(currencies))
                return false;

            foreach (var currency in currencies)
                ChangeCurrency(currency.Settings.Id, -currency.Amount);
            
            return true;
        }

        public bool CanAfford(List<CurrencyData> currencies) =>
            currencies.All(currency => CanAfford(currency.Settings.Id, currency.Amount));

        private void IncreaseCurrency(string currencyId, int amount)
        {
            ChangeCurrency(currencyId, amount);
        }

        private bool CanAfford(string currencyId, int amount)
        {
            if (!_currencies.TryGetValue(currencyId, out var value))
            {
                Debug.Log($"Can't afford currency: {currencyId}x{amount}");
                return false;
            }
            
            if (amount < 0)
                throw new ArgumentException("Amount must be positive for decrease.", nameof(amount));
            
            if(value < amount)
                Debug.Log($"Can't afford currency: {currencyId}x{amount}");
            
            return value >= amount;
        }

        private void ChangeCurrency(string currencyId, int amount)
        {
            if(!_currencies.ContainsKey(currencyId))
                throw new Exception($"Currency {currencyId} not found!");
            
            _currencies[currencyId] += amount;
            CurrencyChanged?.Invoke(currencyId, amount);
        }
    }
}