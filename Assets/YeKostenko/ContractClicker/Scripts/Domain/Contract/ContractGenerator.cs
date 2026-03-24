using System.Collections.Generic;

using UnityEngine;

using YeKostenko.CoreKit.Extensions;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Data.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public class ContractGenerator : IContractGenerator
    {
        private readonly IGameConfigProvider _configProvider;
        private readonly IContractModifierService _contractModifierService;
        private readonly IWorkController _workController;

        private ContractsConfig _config;
        private HashSet<string> _contractNames;

        public ContractGenerator(IGameConfigProvider configProvider, IWorkController workController,
            IContractModifierService contractModifierService)
        {
            _configProvider = configProvider;
            _workController = workController;
            _contractModifierService = contractModifierService;
        }

        public List<ContractDefinition> GenerateContracts(int count)
        {
            _config ??= _configProvider.GetGameConfig().ContractsConfig;

            _contractNames = new HashSet<string>();

            List<ContractDefinition> contracts = new List<ContractDefinition>
            {
                GenerateContract(ContractDifficulty.Easy),
                GenerateContract(ContractDifficulty.Medium),
                GenerateContract(ContractDifficulty.Hard)
            };

            _contractNames.Clear();
            return contracts;
        }

        private ContractDefinition GenerateContract(ContractDifficulty difficulty)
        {
            float timeToComplete = _config.TimeToComplete;
            float rewardMultiplier = _config.RewardMultiplier;
            float rewardPowerExponent = _config.RewardPowerExponent;

            timeToComplete = RandomizeTimeToComplete(timeToComplete);

            int id = UnityEngine.Random.Range(1, int.MaxValue);
            string name = GetUniqueContractName();

            float totalWorkPerSecond = GetManualWorkPerSecond() + GetAutomaticWorkPerSecond();
            float playerPower = totalWorkPerSecond;
            int progress = Mathf.CeilToInt(playerPower * timeToComplete * GetDifficultyMultiplier(difficulty));
            int reward = Mathf.CeilToInt(Mathf.Pow(progress, rewardPowerExponent) * rewardMultiplier);
            reward = RoundToDecimal(reward);

            ModifierSetConfig modifiersSet = _contractModifierService.GetRandomModifiersSet(difficulty);

            return new ContractDefinition(id, name, progress, reward, modifiersSet);
        }

        private int RoundToDecimal(int reward)
        {
            int roundedReward = Mathf.RoundToInt(reward / 10f) * 10;
            return roundedReward;
        }

        private float RandomizeTimeToComplete(float timeToComplete)
        {
            float randomFactor = _config.TimeToCompleteRange.GetRandomValue();
            return timeToComplete * randomFactor;
        }

        private float GetDifficultyMultiplier(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => 0.75f,
                ContractDifficulty.Medium => 1f,
                ContractDifficulty.Hard => 1.35f,
                _ => 1f
            };
        }

        private string GetUniqueContractName()
        {
            string name;
            do
            {
                name = _config.ContractNames.GetRandomElement();
            } while (_contractNames.Contains(name));

            _contractNames.Add(name);
            return name;
        }

        private float GetManualWorkPerSecond()
        {
            IManualWorker worker = _workController.ManualWorker;
            float clickPower = worker.GetStat(StatId.ClickPower, true, false);

            return clickPower;
        }

        private float GetAutomaticWorkPerSecond()
        {
            float total = 0f;
            foreach (IAutomaticWorker worker in _workController.AutomaticWorkers)
            {
                float yieldPerTick = worker.GetStat(StatId.YieldPerTick, true, false);
                float tickInterval = worker.GetStat(StatId.TickInterval);
                total += yieldPerTick / tickInterval;
            }

            return total;
        }
    }
}