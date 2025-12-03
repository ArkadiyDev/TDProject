using System;
using System.Collections.Generic;
using Economy.Currencies;

namespace Economy.Wallets
{
    public interface IWalletService
    {
        event Action<string, int> CurrencyChanged;
        int GetCurrency(string currencyId);
        void IncreaseCurrencies(List<CurrencyData> currencies);
        bool TryDecreaseCurrencies(List<CurrencyData> currencies);
        bool CanAfford(List<CurrencyData> currencies);
    }
}