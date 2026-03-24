using System;
using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Domain.Economy
{
    public class PlayerWallet : IPlayerWallet
    {
        public event Action<CurrencyType, int> BalanceChanged;

        private readonly Dictionary<CurrencyType, Currency> _currencies = new();

        public void Initialize(IEnumerable<Currency> currencies)
        {
            foreach (Currency currency in currencies)
            {
                if (currency != null)
                {
                    _currencies[currency.Type] = currency;
                }
            }
        }

        public int GetBalance(CurrencyType currencyType)
        {
            Currency currency = GetCurrencyInternal(currencyType);
            return currency.Amount;
        }

        public void AddFunds(CurrencyType currencyType, int amount)
        {
            Currency currency = GetCurrencyInternal(currencyType);
            currency.AddAmount(amount);
            BalanceChanged?.Invoke(currencyType, currency.Amount);
        }

        public bool TrySpendFunds(CurrencyType currencyType, int amount)
        {
            Currency currency = GetCurrencyInternal(currencyType);

            if (currency.Amount >= amount)
            {
                currency.SubtractAmount(amount);
                BalanceChanged?.Invoke(currencyType, currency.Amount);
                return true;
            }

            return false;
        }

        public ICurrency GetCurrency(CurrencyType currencyType)
        {
            if (_currencies.TryGetValue(currencyType, out Currency currency))
            {
                return currency;
            }

            throw new KeyNotFoundException($"Currency of type {currencyType} not found in the wallet.");
        }

        private Currency GetCurrencyInternal(CurrencyType currencyType)
        {
            if (_currencies.TryGetValue(currencyType, out Currency currency))
            {
                return currency;
            }

            throw new KeyNotFoundException($"Currency of type {currencyType} not found in the wallet.");
        }
    }
}