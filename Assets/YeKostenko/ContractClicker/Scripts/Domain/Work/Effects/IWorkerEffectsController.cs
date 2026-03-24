﻿using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Work.Effects
{
    public interface IWorkerEffectsController
    {
        IReadOnlyList<ActiveEffect> ActiveEffects { get; }
        void ApplyEffect(EffectDefinition effectDefinition);
        void Tick(float deltaTime);

        float GetModifiedStatValue(StatId statId, float baseValue);
    }
}