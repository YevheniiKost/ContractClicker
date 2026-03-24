using System;
using UnityEngine;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    [Serializable]
    public class ProgressMultiplierConfig : ModifierConfigBase
    {
        [SerializeField]
        [Range(0.1f, 10f)]
        private float _multiplier = 1.5f;

        public float Multiplier => _multiplier;
        public override string ModifierType => "ProgressMultiplier";
        public override string GetDescription() =>
            $"Progress + {Multiplier * 100}%";
    }

    [Serializable]
    public class ProgressBonusConfig : ModifierConfigBase
    {
        [SerializeField]
        [Range(1f, 100f)]
        private float _bonusAmount = 10f;

        public float BonusAmount => _bonusAmount;
        public override string ModifierType => "ProgressBonus";
        public override string GetDescription() =>
            $"Each progress +{BonusAmount}%";
    }

    [Serializable]
    public class WorkTypeRestrictionConfig : ModifierConfigBase
    {
        [SerializeField]
        private ProgressType _allowedType = ProgressType.Manual;

        public ProgressType AllowedType => _allowedType;
        public override string ModifierType => "WorkTypeRestriction";
        public override string GetDescription() =>
            $"Only {AllowedType} progress.";
    }

    [Serializable]
    public class RewardMultiplierConfig : ModifierConfigBase
    {
        [SerializeField]
        [Range(0.1f, 10f)]
        private float _multiplier = 1.5f;

        public float Multiplier => _multiplier;
        public override string ModifierType => "RewardMultiplier";
        public override string GetDescription() =>
            $"Reward X{Multiplier}";
    }

    [Serializable]
    public class RewardBonusConfig : ModifierConfigBase
    {
        [SerializeField]
        [Min(0)]
        private int _bonusAmount = 50;

        public int BonusAmount => _bonusAmount;
        public override string ModifierType => "RewardBonus";
        public override string GetDescription() =>
            $"Reward +{BonusAmount}.";
    }

    [Serializable]
    public class TimeLimitConfig : ModifierConfigBase
    {
        [SerializeField]
        [Range(10f, 600f)]
        private float _timeLimitInSeconds = 60f;

        public float TimeLimitInSeconds => _timeLimitInSeconds;
        public override string ModifierType => "TimeLimit";
        public override string GetDescription() =>
            $"Complete within {TimeLimitInSeconds} s.";
    }

    [Serializable]
    public class TimeBonusRewardConfig : ModifierConfigBase
    {
        [SerializeField] [Range(5f, 300f)] private float _bonusTimeInSeconds = 30f;
        [SerializeField] [Range(1.1f, 5f)] private float _bonusMultiplier = 2f;

        public float BonusTimeInSeconds => _bonusTimeInSeconds;
        public float BonusMultiplier => _bonusMultiplier;
        public override string ModifierType => "TimeBonusReward";
        public override string GetDescription() =>
            $"Complete within {BonusTimeInSeconds} s to earn X{BonusMultiplier} reward";
    }

    [Serializable]
    public class ComboProgressConfig : ModifierConfigBase
    {
        [SerializeField]
        [Range(2, 20)]
        private int _maxCombo = 10;
        [SerializeField]
        [Range(0.05f, 1f)]
        private float _comboBonus = 0.2f;
        [SerializeField]
        [Range(0.5f, 10f)]
        private float _comboResetTime = 2f;

        public int MaxCombo => _maxCombo;
        public float ComboBonus => _comboBonus;
        public float ComboResetTime => _comboResetTime;
        public override string ModifierType => "ComboProgress";

        public override string GetDescription() =>
            $"Progress X{ComboBonus} for work up to {MaxCombo}. Reset {ComboResetTime} s.";
    }
}
