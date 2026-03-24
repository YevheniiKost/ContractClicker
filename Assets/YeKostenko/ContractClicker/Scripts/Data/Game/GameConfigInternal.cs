using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

using YeKostenko.CoreKit.Structs;

using Object = UnityEngine.Object;

namespace YeKostenko.ContractClicker.Data.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ContractClicker/Game/GameConfig")]
    public class GameConfigInternal : ScriptableObject, IGameConfig
    {
        public static async UniTask<GameConfigInternal> Load()
        {
            Object asset = await Resources.LoadAsync<GameConfigInternal>("GameConfig");
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            return asset as GameConfigInternal;
        }

        [Header("Manual Worker")]
        [SerializeField]
        [Range(0f, 1f)]
        private float _manualWorkerCriticalChance;
        [SerializeField]
        [Min(0f)]
        private float _manualWorkerCriticalMultiplier;
        [SerializeField]
        [Min(0f)]
        private float _manualWorkerLevelLinearProgressionFactor;
        [SerializeField]
        [Min(1f)]
        private float _manualWorkerPowerLevelProgressionFactor;
        [SerializeField]
        [Min(0f)]
        private float _manualWorkerClickPower;
        [SerializeField]
        [Min(0f)]
        private float _manualWorkerClickInterval;
        [SerializeField]
        [Min(0f)]
        private int _manualWorkerInitialUpgradePrice;
        [SerializeField]
        [Min(0f)]
        private float _manualWorkerPriceUpgradeMultiplier;
        [SerializeField]
        private List<StatModificationConfigSerializable> _manualWorkerStatsModificationConfigs;

        [Header("Auto Worker")]
        [SerializeField]
        [Range(0f, 1f)]
        private float _autoWorkerCriticalChance;
        [SerializeField]
        [Min(0f)]
        private float _autoWorkerCriticalMultiplier;
        [SerializeField]
        [Min(0f)]
        private float _autoWorkerLevelLinearProgressionFactor;
        [SerializeField]
        [Min(1f)]
        private float _autoWorkerPowerLevelProgressionFactor;
        [SerializeField]
        [Min(0f)]
        private float _autoWorkerYieldPerInterval;
        [SerializeField]
        [Min(0f)]
        private float _autoWorkerYieldInterval;
        [SerializeField]
        [Min(0f)]
        private int _autoWorkerInitialUpgradePrice = 150;
        [SerializeField]
        [Min(0f)]
        private float _autoWorkerPriceUpgradeMultiplier = 1.2f;
        [SerializeField]
        private List<StatModificationConfigSerializable> _autoWorkerStatsModificationConfigs;

        [Header("Player")]
        [SerializeField]
        [Min(0)]
        private int _initialGold = 100;

        [Header("Slots")]
        [SerializeField]
        [Min(1)]
        private int _initialWorkerSlots = 1;
        [SerializeField]
        [Min(1)]
        private int _maxWorkerSlots = 5;
        [SerializeField]
        [Min(1)]
        private int _workerSlotInitialPrice = 200;
        [SerializeField]
        [Min(1)]
        private float _workerSlotPriceMultiplier = 2.5f;
        [SerializeField]
        [Min(1)]
        private int _initialContractSlots = 1;
        [SerializeField]
        [Min(1)]
        private int _maxContractSlots = 1;
        [SerializeField]
        [Min(1)]
        private int _contractSlotInitialPrice = 200;
        [SerializeField]
        [Min(1)]
        private float _contractSlotPriceMultiplier = 2.5f;

        [Header("Contracts")]
        [SerializeField]
        private string[] _contractNames;
        [SerializeField]
        [Min(1)]
        private float _contractTimeToComplete;
        [SerializeField]
        [Min(0)]
        private float _contractRewardMultiplier;
        [SerializeField]
        [Min(0)]
        private float _contractRewardPowerExponent;
        [SerializeField]
        [Min(0)]
        private float _easyContractDifficultyMultiplier;
        [SerializeField]
        [Min(0)]
        private float _mediumContractDifficultyMultiplier;
        [SerializeField]
        [Min(0)]
        private float _hardContractDifficultyMultiplier;
        [SerializeField]
        private MinMaxValue _contractTimeToCompleteRandomizationRange;

        public WorkerConfig ManualWorkerConfig => new WorkerConfig(
            _manualWorkerCriticalChance,
            _manualWorkerCriticalMultiplier,
            _manualWorkerLevelLinearProgressionFactor,
            _manualWorkerPowerLevelProgressionFactor,
            _manualWorkerClickPower,
            _manualWorkerClickInterval,
            _manualWorkerInitialUpgradePrice,
            _manualWorkerPriceUpgradeMultiplier,
            _manualWorkerStatsModificationConfigs.ConvertAll(x => x.ToStatModificationConfig()));

        public WorkerConfig AutoWorkerConfig  =>  new WorkerConfig(
            _autoWorkerCriticalChance,
            _autoWorkerCriticalMultiplier,
            _autoWorkerLevelLinearProgressionFactor,
            _autoWorkerPowerLevelProgressionFactor,
            _autoWorkerYieldPerInterval,
            _autoWorkerYieldInterval,
            _autoWorkerInitialUpgradePrice,
            _autoWorkerPriceUpgradeMultiplier,
            _autoWorkerStatsModificationConfigs.ConvertAll(x => x.ToStatModificationConfig()));

        public ContractsConfig ContractsConfig => new ContractsConfig(new HashSet<string>(_contractNames),
            _contractTimeToComplete,
            _contractRewardMultiplier,
            _contractRewardPowerExponent,
            _easyContractDifficultyMultiplier,
            _mediumContractDifficultyMultiplier,
            _hardContractDifficultyMultiplier,
            _contractTimeToCompleteRandomizationRange);

        public int InitialGold => _initialGold;

        public int AutoWorkerInitialUpgradePrice => _autoWorkerInitialUpgradePrice;
        public float AutoWorkerPriceUpgradeMultiplier => _autoWorkerPriceUpgradeMultiplier;
        public int ManualWorkerInitialUpgradePrice => _manualWorkerInitialUpgradePrice;
        public float ManualWorkerPriceUpgradeMultiplier => _manualWorkerPriceUpgradeMultiplier;

        public SlotsConfig ContractSlotsConfig => new SlotsConfig(
            _initialContractSlots,
            _maxContractSlots,
            _contractSlotInitialPrice,
            _contractSlotPriceMultiplier);

        public SlotsConfig WorkerSlotsConfig => new SlotsConfig(
            _initialWorkerSlots,
            _maxWorkerSlots,
            _workerSlotInitialPrice,
            _workerSlotPriceMultiplier);
    }
}