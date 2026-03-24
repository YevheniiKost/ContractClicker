namespace YeKostenko.ContractClicker.Domain.Work
{
    public class WorkerStats
    {
        public float CriticalChance { get; }
        public float CriticalMultiplier { get; }

        /// <summary>
        /// Multiplier applied to the clickPower/yieldPerTick based on worker level.
        /// </summary>
        public float LevelLinearProgressionFactor { get; }

        /// <summary>
        /// Exponent applied to the clickPower/yieldPerTick based on worker level.
        /// </summary>
        public float PowerCurveProgressionFactor { get; }


        public WorkerStats(float criticalChance, float criticalMultiplier, float levelLinearProgressionFactor, float powerCurveProgressionFactor)
        {
            CriticalChance = criticalChance;
            CriticalMultiplier = criticalMultiplier;
            LevelLinearProgressionFactor = levelLinearProgressionFactor;
            PowerCurveProgressionFactor = powerCurveProgressionFactor;
        }
    }
}