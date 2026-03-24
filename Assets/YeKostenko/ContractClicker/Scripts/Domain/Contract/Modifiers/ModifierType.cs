namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    public enum ModifierType
    {
        Validation = 0,   // Найвищий пріоритет - спочатку валідуємо
        Progress = 100,   // Модифікуємо прогрес
        Reward = 200,     // Модифікуємо винагороду
        Time = 300,       // Часові модифікатори
        Special = 400     // Спеціальні модифікатори
    }
}

