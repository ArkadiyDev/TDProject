using System;

namespace Economy.Wallets
{
    public interface IWalletService
    {
        event Action<string, int> CurrencyChanged;
        int GetCurrency(string currencyId);
        void IncreaseCurrency(string currencyId, int amount);
        bool TryDecreaseCurrency(string currencyId, int amount);
        bool CanAfford(string currencyId, int amount);
    }
}