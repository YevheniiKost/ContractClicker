using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Domain;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;

namespace YeKostenko.ContractClicker.Examples
{
    /// <summary>
    /// Приклади використання системи модифікаторів
    /// </summary>
    public static class ModifierUsageExamples
    {
        /// <summary>
        /// Приклад 1: Простий контракт з множником прогресу
        /// </summary>
        public static IContract CreateSimpleContract()
        {
            var contract = new Contract(1, "Simple Contract", 100f, 50);

            // Додаємо модифікатор, що подвоює прогрес
            contract.AddModifier(new ProgressMultiplierModifier(2.0f));

            return contract;
        }

        /// <summary>
        /// Приклад 2: Контракт тільки для ручної роботи з бонусом до винагороди
        /// </summary>
        public static IContract CreateManualOnlyContract()
        {
            var contract = new Contract(2, "Manual Only Contract", 100f, 100);

            // Тільки ручна робота
            contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));

            // Бонус до винагороди за складність
            contract.AddModifier(new RewardMultiplierModifier(1.5f));

            return contract;
        }

        /// <summary>
        /// Приклад 3: Експрес контракт з обмеженням часу та бонусом
        /// </summary>
        public static IContract CreateRushContract()
        {
            var contract = new Contract(3, "Rush Contract", 50f, 200);

            // Обмеження часу - 60 секунд
            var timeLimit = new TimeLimitModifier(60f);
            contract.AddModifier(timeLimit);

            // Бонус до винагороди якщо виконано за 30 секунд
            var timeBonus = new TimeBonusRewardModifier(30f, 2.0f);
            contract.AddModifier(timeBonus);

            // Підписка на закінчення часу
            timeLimit.OnExpired += () =>
            {
                // Тут можна обробити закінчення часу
                // Наприклад, автоматично провалити контракт
            };

            return contract;
        }

        /// <summary>
        /// Приклад 4: Комбо контракт
        /// </summary>
        public static IContract CreateComboContract()
        {
            var contract = new Contract(4, "Combo Contract", 200f, 150);

            // Комбо модифікатор
            var combo = new ComboProgressModifier(
                maxCombo: 10,           // Максимум 10x комбо
                comboBonus: 0.2f,       // +20% за кожний рівень
                comboResetTime: 2f      // 2 секунди на підтримку комбо
            );
            contract.AddModifier(combo);

            // Підписка на зміну комбо
            combo.OnComboChanged += (comboLevel) =>
            {
                // Тут можна оновити UI або показати ефект
            };

            return contract;
        }

        /// <summary>
        /// Приклад 5: Складний контракт з декількома модифікаторами
        /// </summary>
        public static IContract CreateComplexContract()
        {
            var contract = new Contract(5, "Complex Contract", 300f, 500);

            // 1. Тільки автоматична робота
            contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Automatic));

            // 2. Бонус +50% до прогресу
            contract.AddModifier(new ProgressMultiplierModifier(1.5f));

            // 3. Фіксований бонус +10 до прогресу
            contract.AddModifier(new ProgressBonusModifier(10f));

            // 4. Обмеження часу - 2 хвилини
            contract.AddModifier(new TimeLimitModifier(120f));

            // 5. Бонус до винагороди +200
            contract.AddModifier(new RewardBonusModifier(200));

            // 6. Множник винагороди x1.3
            contract.AddModifier(new RewardMultiplierModifier(1.3f));

            return contract;
        }

        /// <summary>
        /// Приклад використання контракту з модифікаторами
        /// </summary>
        public static void UsageExample()
        {
            var contract = CreateRushContract();

            // Додаємо прогрес вручну
            contract.AddProgress(10f, ProgressType.Manual);

            // Перевіряємо поточний прогрес
            var progress = contract.CurrentProgress; // 10f

            // Оновлюємо модифікатори (наприклад, кожен кадр)
            contract.UpdateModifiers(0.016f); // ~60 FPS

            // Перевіряємо чи є певний модифікатор
            if (contract.HasModifier<TimeLimitModifier>())
            {
                var timeMod = contract.GetModifier<TimeLimitModifier>();
                var remainingTime = timeMod.RemainingTime;
            }

            // Видаляємо модифікатор
            var modifier = contract.GetModifier<TimeLimitModifier>();
            if (modifier != null)
            {
                contract.RemoveModifier(modifier);
            }

            // Перевіряємо фінальну винагороду (з урахуванням модифікаторів)
            var finalReward = contract.FinalReward;

            // Перевіряємо всі модифікатори
            foreach (var mod in contract.Modifiers)
            {
                var info = mod.ToString();
                // Показуємо інформацію про модифікатор
            }
        }
    }
}

