using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Data.Game
{
    public struct WorkerConfig
    {
        public float CriticalChance { get; }
        public float CriticalMultiplier { get; }
        public float LevelLinearProgressionFactor { get; }
        public float PowerCurveProgressionFactor { get; }
        public float WorkPower { get; }
        public float WorkPauseDuration { get; }

        public int InitialUpgradePrice { get; }
        public float UpgradePriceMultiplier { get; }

        public IReadOnlyList<StatModificationConfig> StatModifications { get; }

        public WorkerConfig(
            float criticalChance,
            float criticalMultiplier,
            float levelLinearProgressionFactor,
            float powerCurveProgressionFactor,
            float workPower,
            float workPauseDuration,
            int initialUpgradePrice,
            float upgradePriceMultiplier,
            IReadOnlyList<StatModificationConfig> statModifications)
        {
            CriticalChance = criticalChance;
            CriticalMultiplier = criticalMultiplier;
            LevelLinearProgressionFactor = levelLinearProgressionFactor;
            PowerCurveProgressionFactor = powerCurveProgressionFactor;
            WorkPower = workPower;
            WorkPauseDuration = workPauseDuration;
            InitialUpgradePrice = initialUpgradePrice;
            UpgradePriceMultiplier = upgradePriceMultiplier;
            StatModifications = statModifications;
        }
    }
}