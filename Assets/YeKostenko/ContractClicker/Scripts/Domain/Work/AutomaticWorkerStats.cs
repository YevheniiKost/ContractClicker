namespace YeKostenko.ContractClicker.Domain.Work
{
    public class AutomaticWorkerStats : WorkerStats
    {
        public AutomaticWorkerStats(
            float criticalChance,
            float criticalMultiplier,
            float levelLinearProgressionFactor,
            float powerCurveProgressionFactor,
            float yieldPerTick,
            float tickInterval) : base(criticalChance,
            criticalMultiplier,
            levelLinearProgressionFactor,
            powerCurveProgressionFactor)
        {
            YieldPerTick = yieldPerTick;
            TickInterval = tickInterval;
        }

        public float YieldPerTick { get; }
        public float TickInterval { get; }
    }
}