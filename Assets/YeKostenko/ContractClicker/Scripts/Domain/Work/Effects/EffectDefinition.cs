namespace YeKostenko.ContractClicker.Domain.Work.Effects
{
    public class EffectDefinition
    {
        public string Id { get; }
        public float DurationInSeconds { get; }
        public StackPolicy StackPolicy { get; }
        public int MaxStacks { get; }
        public StatModification[] Modifications { get; }

        public EffectDefinition(string id, float durationInSeconds, StackPolicy stackPolicy, int maxStacks,
            StatModification[] modifications)
        {
            Id = id;
            DurationInSeconds = durationInSeconds;
            StackPolicy = stackPolicy;
            MaxStacks = maxStacks;
            Modifications = modifications;
        }
    }
}