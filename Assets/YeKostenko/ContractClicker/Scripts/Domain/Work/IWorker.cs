using System;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public interface IWorker
    {
        event Action<float, ProgressType> WorkDone;
        event Action LevelUp;

        ProgressType Type { get; }
        int Level { get; }
        IWorkerEffectsController EffectsController { get; }
        float GetStat(StatId statId, bool withLevel = false, bool withEffects = false);
        float GetNextLevelStat(StatId statId);

        void Tick(float deltaTime);
        void LevelUpWorker();
    }
}