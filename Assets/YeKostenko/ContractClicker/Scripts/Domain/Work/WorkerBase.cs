using System;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Work
{
    internal abstract class WorkerBase
    {
        private int _ticksSinceLastCrit;
        private readonly WorkerConfig _config;

        protected WorkerBase(WorkerConfig config)
        {
            _config = config;
        }

        public int Level { get; protected set; }

        protected float ApplyDeterministicCrit(float work, float critChance, float critMultiplier)
        {
            critChance = Math.Clamp(critChance, 0f, 1f);

            if (critChance <= 0f)
            {
                return work;
            }

            int everyN = (int)Math.Round(1f / critChance);
            if (everyN < 1)
            {
                everyN = 1;
            }

            _ticksSinceLastCrit++;

            if (_ticksSinceLastCrit >= everyN)
            {
                _ticksSinceLastCrit = 0;

                if (critMultiplier < 1f)
                {
                    critMultiplier = 1f;
                }

                return work * critMultiplier;
            }

            return work;
        }

        protected float ApplyStatsUpgrades(StatId statId, float baseValue, int level)
        {
            foreach (StatModificationConfig modConfig in _config.StatModifications)
            {
                if (modConfig.StatId != statId)
                {
                    continue;
                }

                if (modConfig.LevelInterval <= 0)
                {
                    continue;
                }

                int effectiveLevel = Math.Min(level, modConfig.LevelCap);
                int upgradeCount = effectiveLevel / modConfig.LevelInterval;

                if (upgradeCount <= 0)
                {
                    continue;
                }

                for (int i = 0; i < upgradeCount; i++)
                {
                    if (modConfig.Operation == ModificationOperation.Add)
                    {
                        baseValue += modConfig.Value;
                    }
                    else if (modConfig.Operation == ModificationOperation.Mul)
                    {
                        baseValue *= modConfig.Value;
                    }
                    else if (modConfig.Operation == ModificationOperation.Override)
                    {
                        baseValue = modConfig.Value;
                    }
                }
            }

            return baseValue;
        }
    }
}