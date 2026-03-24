namespace YeKostenko.ContractClicker.Domain.Work
{
    public class ManualWorkerStats : WorkerStats
    {
        public ManualWorkerStats(
            float criticalChance,
            float criticalMultiplier,
            float levelLinearProgressionFactor,
            float powerCurveProgressionFactor,
            float clickPower,
            float clickCooldown) : base(criticalChance,
            criticalMultiplier,
            levelLinearProgressionFactor,
            powerCurveProgressionFactor)
        {
            ClickPower = clickPower;
            ClickCooldown = clickCooldown;
        }

        public float ClickPower { get; }
        public float ClickCooldown { get; }
    }
}