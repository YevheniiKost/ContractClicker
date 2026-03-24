namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward
{
    /// <summary>
    /// Модифікатор, що додає фіксований бонус до винагороди
    /// </summary>
    public class RewardBonusModifier : ContractModifierBase, IRewardModifier
    {
        private readonly int _bonusAmount;

        public int BonusAmount => _bonusAmount;

        public RewardBonusModifier(int bonusAmount)
        {
            _bonusAmount = bonusAmount;
            Type = ModifierType.Reward;
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Цей модифікатор не впливає на процес додавання прогресу
            return ModifierResult.Continue();
        }

        public int ModifyReward(int baseReward)
        {
            return baseReward + _bonusAmount;
        }

        public override string ToString()
        {
            return $"Reward +{_bonusAmount}";
        }
    }
}

