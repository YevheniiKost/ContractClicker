using System;
using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Domain.Economy
{
    public interface IPlayerWallet
    {
        event Action<CurrencyType, int> BalanceChanged;

        void Initialize(IEnumerable<Currency> currencies);

        int GetBalance(CurrencyType currencyType);
        void AddFunds(CurrencyType currencyType, int amount);
        bool TrySpendFunds(CurrencyType currencyType, int amount);

        ICurrency GetCurrency(CurrencyType currencyType);
    }
}