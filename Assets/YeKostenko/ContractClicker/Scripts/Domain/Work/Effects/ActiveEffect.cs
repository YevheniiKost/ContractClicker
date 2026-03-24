using UnityEngine;

namespace YeKostenko.ContractClicker.Domain.Work.Effects
{
    public class ActiveEffect
    {
        public EffectDefinition Definition { get; }
        public float RemainingDurationInSeconds { get; private set; }
        public int CurrentStacks { get; private set; }

        public ActiveEffect(EffectDefinition definition)
        {
            Definition = definition;
            RemainingDurationInSeconds = definition.DurationInSeconds;
            CurrentStacks = 1;
        }

        public void Refresh() => RemainingDurationInSeconds = Definition.DurationInSeconds;

        public void AddStack()
        {
            if (Definition.MaxStacks > 0)
            {
                CurrentStacks = Mathf.Min(CurrentStacks + 1, Definition.MaxStacks);
            }
            else
            {
                CurrentStacks++;
            }
        }

        public bool Tick(float deltaTime)
        {
            if(Definition.DurationInSeconds <= 0)
            {
                return false;
            }

            RemainingDurationInSeconds -= deltaTime;
            return RemainingDurationInSeconds <= 0;
        }
    }
}