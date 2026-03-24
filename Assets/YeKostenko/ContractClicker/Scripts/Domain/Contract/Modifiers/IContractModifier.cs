namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    /// <summary>
    /// Базовий інтерфейс для всіх модифікаторів контрактів
    /// </summary>
    public interface IContractModifier
    {
        /// <summary>
        /// Тип модифікатора (визначає базовий пріоритет)
        /// </summary>
        ModifierType Type { get; }

        /// <summary>
        /// Пріоритет виконання (менше = раніше виконується)
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Чи активний модифікатор
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Унікальний ідентифікатор модифікатора
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Викликається при додаванні модифікатора до контракту
        /// </summary>
        void OnApply(IContract contract);

        /// <summary>
        /// Викликається при видаленні модифікатора з контракту
        /// </summary>
        void OnRemove(IContract contract);

        /// <summary>
        /// Обробка контексту модифікатора
        /// </summary>
        ModifierResult Process(ModifierContext context);

        /// <summary>
        /// Оновлення модифікатора (для часових модифікаторів)
        /// </summary>
        void Update(float deltaTime);
    }
}

