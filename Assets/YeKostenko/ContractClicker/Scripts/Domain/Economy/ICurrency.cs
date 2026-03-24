namespace YeKostenko.ContractClicker.Domain.Economy
{
    public interface ICurrency
    {
        string Id { get; }
        string Name { get; }
        int Amount { get; }
        CurrencyType Type { get; }
    }
}