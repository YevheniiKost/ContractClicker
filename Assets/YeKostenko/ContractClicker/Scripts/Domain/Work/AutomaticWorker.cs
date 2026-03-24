using System;

using UnityEngine;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Domain.Work
{
    internal class AutomaticWorker : WorkerBase, IAutomaticWorker
    {
        private float _timeAccumulator;

        public AutomaticWorker(int id, AutomaticWorkerStats stats, int level, WorkerConfig config,
            IWorkerEffectsController effectsController) : base(config)
        {
            Id = id;
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
            Level = level;
            EffectsController = effectsController ?? throw new ArgumentNullException(nameof(effectsController));
        }

        public event Action<float, ProgressType> WorkDone;
        public event Action LevelUp;

        public ProgressType Type => ProgressType.Automatic;
        public IWorkerEffectsController EffectsController { get; }
        public int Id { get; }
        public AutomaticWorkerStats Stats { get; }

        public float GetStat(StatId statId, bool withLevel = false, bool withEffects = false)
        {
            return GetStatInternal(statId, withLevel, withEffects, Level);
        }

        public float GetNextLevelStat(StatId statId)
        {
            return GetStatInternal(statId, true, false, Level + 1);
        }

        public void Tick(float deltaTime)
        {
            EffectsController?.Tick(deltaTime);

            _timeAccumulator += deltaTime;

            float tickInterval = CalculateTickInterval();
            if (_timeAccumulator >= tickInterval)
            {
                int ticksToProcess = Mathf.FloorToInt(_timeAccumulator / tickInterval);
                float totalYield = CalculateYieldPerTick(Level) * ticksToProcess;

                WorkDone?.Invoke(totalYield, Type);
                _timeAccumulator -= ticksToProcess * tickInterval;
            }
        }

        public void LevelUpWorker()
        {
            Level++;
            LevelUp?.Invoke();
        }

        private float GetStatInternal(StatId statId, bool withLevel, bool withEffects, int level)
        {
            float baseValue = statId switch
            {
                StatId.YieldPerTick => Stats.YieldPerTick,
                StatId.TickInterval => Stats.TickInterval,
                StatId.CritChance => Stats.CriticalChance,
                StatId.CritMultiplier => Stats.CriticalMultiplier,
                _ => throw new ArgumentOutOfRangeException(nameof(statId), $"StatId {statId} is not supported by AutomaticWorker"),
            };

            if (withLevel)
            {
                if (statId == StatId.YieldPerTick)
                {
                    baseValue = Mathf.Pow(baseValue * (1 + (level * Stats.LevelLinearProgressionFactor)),
                        Stats.PowerCurveProgressionFactor);
                }
            }

            baseValue = ApplyStatsUpgrades(statId, baseValue, level);

            if (withEffects)
            {
                baseValue = EffectsController.GetModifiedStatValue(statId, baseValue);
            }

            return baseValue;
        }

        private float CalculateYieldPerTick(int level)
        {
            float baseYieldPerTick = Stats.YieldPerTick;
            float levelYieldPerTick = Mathf.Pow(baseYieldPerTick * (1 + (level * Stats.LevelLinearProgressionFactor)),
                Stats.PowerCurveProgressionFactor);
            float effectsClickPerTick = EffectsController.GetModifiedStatValue(StatId.YieldPerTick, levelYieldPerTick);
            float critYieldPerTick = ApplyDeterministicCrit(effectsClickPerTick,
                EffectsController.GetModifiedStatValue(StatId.CritChance, Stats.CriticalChance),
                EffectsController.GetModifiedStatValue(StatId.CritMultiplier, Stats.CriticalMultiplier));

            return critYieldPerTick;
        }

        private float CalculateTickInterval()
        {
            float baseInterval = Stats.TickInterval;
            float modifiedInterval = EffectsController.GetModifiedStatValue(StatId.TickInterval, baseInterval);

            return modifiedInterval;
        }
    }
}