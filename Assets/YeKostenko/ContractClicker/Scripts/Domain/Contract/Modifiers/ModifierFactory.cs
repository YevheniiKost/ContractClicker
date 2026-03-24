using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Modifiers;

using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    public class ModifierFactory
    {
        public static ModifierFactory Default { get; } = new ModifierFactory();

        public IContractModifier CreateFromConfig(ModifierConfigBase config)
        {
            if (config == null)
            {
                return null;
            }

            return config switch
            {
                ProgressMultiplierConfig cfg => new ProgressMultiplierModifier(cfg.Multiplier),
                ProgressBonusConfig cfg => new ProgressBonusModifier(cfg.BonusAmount),
                WorkTypeRestrictionConfig cfg => new WorkTypeRestrictionModifier(cfg.AllowedType),
                RewardMultiplierConfig cfg => new RewardMultiplierModifier(cfg.Multiplier),
                RewardBonusConfig cfg => new RewardBonusModifier(cfg.BonusAmount),
                TimeLimitConfig cfg => new TimeLimitModifier(cfg.TimeLimitInSeconds),
                TimeBonusRewardConfig cfg => new TimeBonusRewardModifier(cfg.BonusTimeInSeconds, cfg.BonusMultiplier),
                ComboProgressConfig cfg => new ComboProgressModifier(cfg.MaxCombo, cfg.ComboBonus, cfg.ComboResetTime),
                _ => null
            };
        }

        public List<IContractModifier> CreateFromSet(ModifierSetConfig set)
        {
            if (set == null)
            {
                return new List<IContractModifier>();
            }

            List<IContractModifier> modifiers = new List<IContractModifier>();

            foreach (ModifierConfigBase config in set.GetActiveModifierConfigs())
            {
                IContractModifier modifier = CreateFromConfig(config);
                if (modifier != null)
                {
                    modifiers.Add(modifier);
                }
            }

            return modifiers;
        }

        public List<IContractModifier> CreateFromSets(IEnumerable<ModifierSetConfig> sets)
        {
            List<IContractModifier> modifiers = new List<IContractModifier>();

            foreach (ModifierSetConfig set in sets)
            {
                modifiers.AddRange(CreateFromSet(set));
            }

            return modifiers;
        }
    }
}


