namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward
{
    /// <summary>
    /// Модифікатор, що множить винагороду на коефіцієнт
    /// </summary>
    public class RewardMultiplierModifier : ContractModifierBase, IRewardModifier
    {
        private readonly float _multiplier;

        public float Multiplier => _multiplier;

        public RewardMultiplierModifier(float multiplier)
        {
            _multiplier = multiplier;
            Type = ModifierType.Reward;
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Цей модифікатор не впливає на процес додавання прогресу
            return ModifierResult.Continue();
        }

        public int ModifyReward(int baseReward)
        {
            return (int)(baseReward * _multiplier);
        }

        public override string ToString()
        {
            return $"Reward x{_multiplier:F2}";
        }
    }
}

