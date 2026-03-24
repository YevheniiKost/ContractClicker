using System;

using UnityEngine;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Domain.Work
{
    internal class ManualWorker : WorkerBase, IManualWorker
    {
        private readonly ManualWorkerStats _stats;

        private float _lastClickTime = -Mathf.Infinity;

        public ManualWorker(ManualWorkerStats stats, int level, WorkerConfig config, IWorkerEffectsController effectsController) : base(config)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats), "ManualWorkerModelStats cannot be null");
            EffectsController = effectsController ?? throw new ArgumentNullException(nameof(effectsController), "EffectsController cannot be null");
            Level = level;
        }

        public event Action<float, ProgressType> WorkDone;
        public event Action LevelUp;

        public ProgressType Type => ProgressType.Manual;

        public IWorkerEffectsController EffectsController { get; }

        public void ProcessClick()
        {
            if((Time.time - _lastClickTime) < _stats.ClickCooldown)
            {
                return;
            }

            WorkDone?.Invoke(GetCurrentWorkPerClick(), Type);
            _lastClickTime = Time.time;
        }

        public float GetStat(StatId statId, bool withLevel = false, bool withEffects = false)
        {
            return GetStatInternal(statId, withLevel, withEffects, Level);
        }

        public float GetNextLevelStat(StatId statId)
        {
            return GetStatInternal(statId, withLevel: true, withEffects: false, level: Level + 1);
        }

        public void Tick(float deltaTime) => EffectsController?.Tick(deltaTime);

        public void LevelUpWorker()
        {
            Level++;
            LevelUp?.Invoke();
        }

        private float GetStatInternal(StatId statId, bool withLevel, bool withEffects, int level)
        {
            float baseValue = statId switch
            {
                StatId.ClickPower => _stats.ClickPower,
                StatId.CritChance => _stats.CriticalChance,
                StatId.CritMultiplier => _stats.CriticalMultiplier,
                StatId.TickInterval => _stats.ClickCooldown,
                _ => throw new ArgumentOutOfRangeException(nameof(statId), $"StatId {statId} is not supported by ManualWorker"),
            };

            if(withLevel)
            {
                if(statId == StatId.ClickPower)
                {
                    baseValue = GetClickPowerBasedOnLevel(baseValue, level);
                }
            }

            baseValue = ApplyStatsUpgrades(statId, baseValue, level);

            if(withEffects)
            {
                baseValue = EffectsController.GetModifiedStatValue(statId, baseValue);
            }

            return baseValue;
        }

        private float GetCurrentWorkPerClick()
        {
            float baseClickPower = _stats.ClickPower;
            float levelClickPower = GetClickPowerBasedOnLevel(baseClickPower, Level);
            float effectsClickPower = EffectsController.GetModifiedStatValue(StatId.ClickPower, levelClickPower);
            float critClickPower = ApplyDeterministicCrit(effectsClickPower,
                EffectsController.GetModifiedStatValue(StatId.CritChance, _stats.CriticalChance),
                EffectsController.GetModifiedStatValue(StatId.CritMultiplier, _stats.CriticalMultiplier));

            return critClickPower;
        }

        private float GetClickPowerBasedOnLevel(float baseClickPower, int level)
        {
            return Mathf.Pow(baseClickPower * (1f + (level * _stats.LevelLinearProgressionFactor)),
                _stats.PowerCurveProgressionFactor);
        }
    }
}