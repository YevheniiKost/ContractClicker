﻿using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Work.Effects
{
    internal class WorkerEffectsController   : IWorkerEffectsController
    {
        private readonly List<ActiveEffect> _activeEffects = new List<ActiveEffect>();

        public IReadOnlyList<ActiveEffect> ActiveEffects => _activeEffects;

        public void ApplyEffect(EffectDefinition effectDefinition)
        {
            ActiveEffect existingEffect = _activeEffects.Find(e => e.Definition.Id == effectDefinition.Id);

            if (existingEffect == null)
            {
                _activeEffects.Add(new ActiveEffect(effectDefinition));
                return;
            }

            switch (effectDefinition.StackPolicy)
            {
                case StackPolicy.Stack:
                    existingEffect.AddStack();
                    existingEffect.Refresh();
                    break;
                case StackPolicy.RefreshDuration:
                    existingEffect.Refresh();
                    break;
                case StackPolicy.Replace:
                    _activeEffects.Remove(existingEffect);
                    _activeEffects.Add(new ActiveEffect(effectDefinition));
                    break;
                case StackPolicy.IgnoreIfPresent:
                default:
                    break;
            }
        }

        public void Tick(float deltaTime)
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i].Tick(deltaTime))
                {
                    _activeEffects.RemoveAt(i);
                }
            }
        }

        private struct Aggregator
        {
            public bool HasOverride;
            public float OverrideValue;
            public int OverridePriority;

            public float AdditiveSum;
            public float MulFactor;

            public static Aggregator Default => new Aggregator
            {
                HasOverride = false,
                OverridePriority = int.MinValue,
                AdditiveSum = 0f,
                MulFactor = 1f
            };
        }

        public float GetModifiedStatValue(StatId statId, float baseValue)
        {
            Aggregator agg = Aggregator.Default;

            foreach (ActiveEffect activeEffect in _activeEffects)
            {
                foreach (StatModification modification in activeEffect.Definition.Modifications)
                {
                    if (modification.Stat != statId)
                    {
                        continue;
                    }

                    float totalModificationValue = modification.Value * activeEffect.CurrentStacks;

                    switch (modification.Operation)
                    {
                        case ModificationOperation.Add:
                            agg.AdditiveSum += totalModificationValue;
                            break;

                        case ModificationOperation.Mul:
                            agg.MulFactor *= PowFast(totalModificationValue, activeEffect.CurrentStacks);
                            break;

                        case ModificationOperation.Override:

                            if (!agg.HasOverride || modification.Priority > agg.OverridePriority)
                            {
                                agg.HasOverride = true;
                                agg.OverrideValue = totalModificationValue;
                                agg.OverridePriority = modification.Priority;
                            }
                            break;
                    }
                }
            }

            float modifiedValue = agg.HasOverride ? agg.OverrideValue : baseValue;
            modifiedValue = (modifiedValue + agg.AdditiveSum) * agg.MulFactor;
            return modifiedValue;
        }

        private static float PowFast(float factor, int exp)
        {
            float r = 1f;
            for (int i = 0; i < exp; i++)
            {
                r *= factor;
            }
            return r;
        }
    }
}