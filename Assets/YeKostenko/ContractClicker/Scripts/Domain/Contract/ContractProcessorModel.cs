using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;

using YevheniiKostenko.CoreKit.Time;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public class ContractProcessorModel : IContractProcessorModel, ITimeListener
    {
        private readonly IGameConfigProvider _configProvider;

        private int _availableContractSlots;
        private List<IContract> _activeContracts = new List<IContract>();
        private SlotsConfig _slotsConfig;

        public ContractProcessorModel(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public event Action<IContract> NewContractStarted;
        public event Action<IContract> ContractCompleted;
        public event Action<IContract> ProgressAdded;
        public event Action<IContract> ContractSkipped;

        public List<IContract> ActiveContracts => _activeContracts;
        public int UnlockedContractSlots => _availableContractSlots;
        public int MaxContractSlots => _slotsConfig.MaxSlots;

        public void Initialize()
        {
            _slotsConfig = _configProvider.GetGameConfig().ContractSlotsConfig;
            _availableContractSlots = _slotsConfig.InitialSlots;
        }

        public bool StartNewContract(IContract contract)
        {
            if (GetActiveContractById(contract.Id) != null)
            {
                return false;
            }

            if (ActiveContracts.Count >= UnlockedContractSlots)
            {
                return false;
            }

            _activeContracts.Add(contract);
            NewContractStarted?.Invoke(contract);
            return true;
        }

        public void AddProgressToContract(int contractId, float amount, ProgressType progressType)
        {
            IContract contract = GetActiveContractById(contractId);
            if (contract == null)
            {
                throw new Exception($"Contract with ID {contractId} not found among active contracts.");
            }

            contract.AddProgress(amount, progressType);
            ProgressAdded?.Invoke(contract);

            if (contract.IsCompleted)
            {
                ContractCompleted?.Invoke(contract);
            }
        }

        public void ClearCompletedContracts()
        {
            _activeContracts.RemoveAll(contract => contract.IsCompleted);
        }

        public void ClearContract(int contractId)
        {
            IContract contract = GetActiveContractById(contractId);
            if (contract == null)
            {
                throw new Exception($"Contract with ID {contractId} not found among active contracts.");
            }

            _activeContracts.Remove(contract);
        }

        public void SkipContract(int contractId)
        {
            IContract contract = GetActiveContractById(contractId);
            if (contract == null)
            {
                throw new Exception($"Contract with ID {contractId} not found among active contracts.");
            }

            _activeContracts.Remove(contract);
            ContractSkipped?.Invoke(contract);
        }

        public void OpenNewContractSlot() => _availableContractSlots = Math.Min(_availableContractSlots + 1, _slotsConfig.MaxSlots);

        private IContract GetActiveContractById(int contractId)
        {
            foreach (IContract activeContract in  _activeContracts)
            {
                if (activeContract.Id == contractId)
                {
                    return activeContract;
                }
            }

            return null;
        }

        public void Update(float deltaTime)
        {
            foreach (IContract contract in _activeContracts)
            {
                contract.UpdateModifiers(deltaTime);
            }
        }
    }
}