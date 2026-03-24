using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.ContractClicker.Domain.Work;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class ApplyBoosterUseCase : IApplyBoosterUseCase
    {
        private readonly IWorkController _workController;
        private readonly IBoosterManager _boosterManager;

        public ApplyBoosterUseCase(IWorkController workController, IBoosterManager boosterManager)
        {
            _workController = workController;
            _boosterManager = boosterManager;
        }

        public void Execute(BoosterConfig config)
        {
            if (config.StatModifications == null || config.StatModifications.Count == 0)
            {
                return;
            }

            Dictionary<BoosterTarget, List<StatModification>> modificationsByTarget =
                new Dictionary<BoosterTarget, List<StatModification>>();

            IReadOnlyList<BoosterStatModificationData> statMods = config.StatModifications;

            for (int i = 0; i < statMods.Count; i++)
            {
                BoosterStatModificationData data = statMods[i];

                if (!modificationsByTarget.ContainsKey(data.Target))
                {
                    modificationsByTarget[data.Target] = new List<StatModification>();
                }

                modificationsByTarget[data.Target].Add(
                    new StatModification(data.Stat, data.Value, data.Operation));
            }

            foreach (KeyValuePair<BoosterTarget, List<StatModification>> pair in modificationsByTarget)
            {
                string effectId = config.BoosterId + "_" + pair.Key.ToString();
                StatModification[] modifications = pair.Value.ToArray();

                EffectDefinition effectDefinition = new EffectDefinition(
                    effectId,
                    config.DurationInSeconds,
                    StackPolicy.RefreshDuration,
                    maxStacks: 1,
                    modifications);

                ApplyEffectToTarget(pair.Key, effectDefinition);
            }

            _boosterManager.ActivateBooster(config);
        }

        private void ApplyEffectToTarget(BoosterTarget target, EffectDefinition effectDefinition)
        {
            switch (target)
            {
                case BoosterTarget.ManualWorker:
                    ApplyToManualWorker(effectDefinition);
                    break;

                case BoosterTarget.AutomaticWorkers:
                    ApplyToAllAutomaticWorkers(effectDefinition);
                    break;

                case BoosterTarget.AllWorkers:
                    ApplyToManualWorker(effectDefinition);
                    ApplyToAllAutomaticWorkers(effectDefinition);
                    break;
            }
        }

        private void ApplyToManualWorker(EffectDefinition effectDefinition)
        {
            if (_workController.ManualWorker == null)
            {
                return;
            }

            _workController.ManualWorker.EffectsController.ApplyEffect(effectDefinition);
        }

        private void ApplyToAllAutomaticWorkers(EffectDefinition effectDefinition)
        {
            List<IAutomaticWorker> automaticWorkers = _workController.AutomaticWorkers;

            if (automaticWorkers == null)
            {
                return;
            }

            for (int i = 0; i < automaticWorkers.Count; i++)
            {
                automaticWorkers[i].EffectsController.ApplyEffect(effectDefinition);
            }
        }
    }
}
