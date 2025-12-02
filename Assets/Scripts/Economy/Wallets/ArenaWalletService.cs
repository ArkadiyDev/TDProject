using System;
using System.Collections.Generic;
using Economy.Currencies;

namespace Economy.Wallets
{
    public class ArenaWalletService : IWalletService
    {
        public event Action<string, int> CurrencyChanged;

        private readonly Dictionary<string, int> _currencies = new();
        
        public ArenaWalletService(CurrencySettingsRoster currencySettingsRoster, List<CurrencyData> startCurrencies)
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

        public void IncreaseCurrency(string currencyId, int amount)
        {
            ChangeCurrency(currencyId, amount);
        }
        
        public bool TryDecreaseCurrency(string currencyId, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive for decrease.", nameof(amount));

            if (!CanAfford(currencyId, amount))
                return false;

            ChangeCurrency(currencyId, -amount);
            
            return true;
        }

        public bool CanAfford(string currencyId, int amount)
        {
            if(!_currencies.TryGetValue(currencyId, out var value))
                return false;
            
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