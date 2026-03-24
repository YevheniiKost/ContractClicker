using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Work.Effects
{
    public class StatModification
    {
        public StatId Stat { get; }
        public float Value { get; }
        public ModificationOperation Operation { get; }

        public int Priority { get; }

        public StatModification(StatId stat, float value, ModificationOperation operation, int priority = 0)
        {
            Stat = stat;
            Value = value;
            Operation = operation;
            Priority = priority;
        }
    }
}