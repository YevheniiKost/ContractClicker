namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress
{
    /// <summary>
    /// Модифікатор, що додає фіксований бонус до прогресу
    /// </summary>
    public class ProgressBonusModifier : ContractModifierBase
    {
        private readonly float _bonusAmount;

        public float BonusAmount => _bonusAmount;

        public ProgressBonusModifier(float bonusAmount)
        {
            _bonusAmount = bonusAmount;
            Type = ModifierType.Progress;
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Додаємо фіксований бонус
            context.ModifiedAmount += _bonusAmount;
            return ModifierResult.Continue();
        }

        public override string ToString()
        {
            return $"Progress +{_bonusAmount:F1}";
        }
    }
}

