namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress
{
    /// <summary>
    /// Модифікатор, що множить прогрес на заданий коефіцієнт
    /// </summary>
    public class ProgressMultiplierModifier : ContractModifierBase
    {
        private readonly float _multiplier;

        public float Multiplier => _multiplier;

        public ProgressMultiplierModifier(float multiplier)
        {
            _multiplier = multiplier;
            Type = ModifierType.Progress;
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Множимо прогрес на коефіцієнт
            context.ModifiedAmount *= _multiplier;
            return ModifierResult.Continue();
        }

        public override string ToString()
        {
            return $"Progress x{_multiplier:F2}";
        }
    }
}

