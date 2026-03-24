namespace YeKostenko.ContractClicker.Data.Game
{
    public class StatModificationConfig
    {
        public StatId StatId { get; }
        public ModificationOperation Operation { get; }
        public float Value { get; }
        public int LevelInterval { get; }
        public int LevelCap { get; }

        public StatModificationConfig(StatId statId, ModificationOperation operation, float value, int levelInterval,
            int levelCap)
        {
            StatId = statId;
            Operation = operation;
            Value = value;
            LevelInterval = levelInterval;
            LevelCap = levelCap;
        }
    }
}