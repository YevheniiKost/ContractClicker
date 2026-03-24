namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    /// <summary>
    /// Інтерфейс для модифікаторів, що змінюють винагороду
    /// </summary>
    public interface IRewardModifier : IContractModifier
    {
        /// <summary>
        /// Модифікувати винагороду контракту
        /// </summary>
        /// <param name="baseReward">Базова винагорода</param>
        /// <returns>Модифікована винагорода</returns>
        int ModifyReward(int baseReward);
    }
}

