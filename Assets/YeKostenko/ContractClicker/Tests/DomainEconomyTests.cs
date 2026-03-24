using System.Collections.Generic;

using NUnit.Framework;

using YeKostenko.ContractClicker.Domain.Economy;

namespace YeKostenko.ContractClicker.Tests
{
    [TestFixture]
    public class DomainEconomyTests
    {
        [Test]
        public void CurrencyTest()
        {
            const string ID = "gold";
            const string Name = "Gold";
            const int InitialAmount = 100;
            const CurrencyType Type = CurrencyType.Gold;

            Currency currency = new Currency(ID, Name, Type, InitialAmount);

            Assert.AreEqual(ID, currency.Id);
            Assert.AreEqual(Name, currency.Name);
            Assert.AreEqual(Type, currency.Type);
            Assert.AreEqual(InitialAmount, currency.Amount);
            currency.AddAmount(50);
            Assert.AreEqual(150, currency.Amount);

            Assert.Throws<System.ArgumentException>(() => currency.AddAmount(-10));

            currency.SubtractAmount(30);
            Assert.AreEqual(120, currency.Amount);

            Assert.Throws<System.ArgumentException>(() => currency.SubtractAmount(-10));
            Assert.Throws<System.ArgumentException>(() => currency.SubtractAmount(200));
        }

        [Test]
        public void PlayerWalletTest()
        {
            int balanceChangedEventCount = 0;

            void BalanceChangedHandler(CurrencyType currencyType, int newBalance)
            {
                balanceChangedEventCount++;
            }

            Currency goldCurrency = new Currency("gold", "Gold", CurrencyType.Gold, 100);
            Currency gemCurrency = new Currency("gem", "Gem", CurrencyType.Gem, 50);

            PlayerWalletFactory factory = new PlayerWalletFactory();
            IPlayerWallet wallet = factory.Create();
            wallet.Initialize(new [] { goldCurrency, gemCurrency });

            wallet.BalanceChanged += BalanceChangedHandler;

            Assert.AreEqual(100, wallet.GetBalance(CurrencyType.Gold));
            Assert.AreEqual(50, wallet.GetBalance(CurrencyType.Gem));

            wallet.AddFunds(CurrencyType.Gold, 50);
            Assert.AreEqual(150, wallet.GetBalance(CurrencyType.Gold));
            Assert.AreEqual(balanceChangedEventCount , 1);

            bool spendResult = wallet.TrySpendFunds(CurrencyType.Gem, 30);
            Assert.IsTrue(spendResult);
            Assert.AreEqual(20, wallet.GetBalance(CurrencyType.Gem));
            Assert.AreEqual(balanceChangedEventCount , 2);

            spendResult = wallet.TrySpendFunds(CurrencyType.Gem, 30);
            Assert.IsFalse(spendResult);
            Assert.AreEqual(20, wallet.GetBalance(CurrencyType.Gem));
            Assert.AreEqual(balanceChangedEventCount , 2);

            ICurrency goldFromWallet = wallet.GetCurrency(CurrencyType.Gold);
            Assert.AreEqual(goldCurrency, goldFromWallet);

            //Test get non-existing currency
            Assert.Throws<KeyNotFoundException>(() => wallet.GetCurrency(CurrencyType.Energy));

            //Test add funds to non-existing currency
            Assert.Throws<KeyNotFoundException>(() => wallet.AddFunds(CurrencyType.Energy, 10));

            //Test try spend funds from non-existing currency
            Assert.Throws<KeyNotFoundException>(() => wallet.TrySpendFunds(CurrencyType.Energy, 10));

            wallet.BalanceChanged -= BalanceChangedHandler;
        }
    }
}