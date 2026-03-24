namespace YeKostenko.ContractClicker.Domain.Economy
{
    public class Currency : ICurrency
    {
        public string Id { get; }
        public string Name { get; }
        public int Amount { get; private set; }
        public CurrencyType Type { get; }

        public Currency(string id, string name, CurrencyType type, int initialAmount = 0)
        {
            Id = id;
            Name = name;
            Type = type;
            Amount = initialAmount;
        }

        public void AddAmount(int amount)
        {
            if (amount < 0)
                throw new System.ArgumentException("Amount to add cannot be negative.", nameof(amount));

            Amount += amount;
        }

        public void SubtractAmount(int amount)
        {
            if (amount < 0)
                throw new System.ArgumentException("Amount to subtract cannot be negative.", nameof(amount));
            if (amount > Amount)
                throw new System.ArgumentException("Cannot subtract more than the current amount.", nameof(amount));

            Amount -= amount;
        }
    }
}