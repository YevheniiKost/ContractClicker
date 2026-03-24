using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Domain.Automation
{
    public class AutoFillModel : IAutoFillModel
    {
        public event Action<int> AutoFillChanged;

        private readonly IContractProcessorModel _contractProcessorModel;
        private readonly IWorkController _workController;

        private Dictionary<int, bool> _autoFillSettings = new Dictionary<int, bool>();

        public AutoFillModel(IContractProcessorModel contractProcessorModel, IWorkController workController)
        {
            _contractProcessorModel = contractProcessorModel;
            _workController = workController;
        }

        public void Initialize()
        {
            _contractProcessorModel.NewContractStarted += OnNewContractStarted;
            _contractProcessorModel.ContractCompleted += OnContractCompleted;
            _contractProcessorModel.ContractSkipped += OnContractSkipped;

            _workController.ManualProgress.ProgressChanged += OnManualProgressChanged;
            _workController.AutomaticProgress.ProgressChanged += OnAutomaticProgressChanged;
        }

        public void SetContractAutoFill(int contractId, bool isEnabled)
        {
            if (!_autoFillSettings.ContainsKey(contractId))
            {
                throw new Exception($"Contract with ID {contractId} not found in auto-fill settings.");
            }

            _autoFillSettings[contractId] = isEnabled;
            AutoFillChanged?.Invoke(contractId);
        }

        public bool IsContractAutoFillEnabled(int contractId) => _autoFillSettings.TryGetValue(contractId, out bool isEnabled) && isEnabled;

        public void Dispose()
        {
            _contractProcessorModel.NewContractStarted -= OnNewContractStarted;
            _contractProcessorModel.ContractCompleted -= OnContractCompleted;
            _contractProcessorModel.ContractSkipped -= OnContractSkipped;
        }

        private void OnAutomaticProgressChanged()
        {
            float availableProgress = _workController.AutomaticProgress.Amount;
            if (availableProgress <= 0)
            {
                return;
            }

            if (AddProgress(availableProgress, ProgressType.Automatic))
            {
                _workController.AutomaticProgress.SpendProgress(availableProgress);
            }
        }

        private void OnManualProgressChanged()
        {
            float availableProgress = _workController.ManualProgress.Amount;
            if (availableProgress <= 0)
            {
                return;
            }

            if (AddProgress(availableProgress, ProgressType.Manual))
            {
                _workController.ManualProgress.SpendProgress(availableProgress);
            }
        }

        private bool AddProgress(float availableProgress, ProgressType progressType)
        {
            if (availableProgress > 0)
            {
                List<IContract> contractsNeedToFill = new  List<IContract>();
                foreach (IContract contract in _contractProcessorModel.ActiveContracts)
                {
                    if (_autoFillSettings.TryGetValue(contract.Id, out bool isAutoFillEnabled) && isAutoFillEnabled
                        && CanFillContract(contract, progressType))
                    {
                        contractsNeedToFill.Add(contract);
                    }
                }

                if(contractsNeedToFill.Count == 0)
                {
                    return false;
                }

                float progressPerContract = availableProgress / contractsNeedToFill.Count;
                foreach (IContract contract in contractsNeedToFill)
                {
                    _contractProcessorModel.AddProgressToContract(contract.Id, progressPerContract, progressType);
                }

                return true;
            }

            return false;
        }

        private bool CanFillContract(IContract contract, ProgressType progressType)
        {
            if (contract.HasModifier<WorkTypeRestrictionModifier>())
            {
                WorkTypeRestrictionModifier modifier = contract.GetModifier<WorkTypeRestrictionModifier>();
                return modifier.IsActive && modifier.AllowedType == progressType;
            }

            return true;
        }

        private void OnNewContractStarted(IContract contract)
        {
            _autoFillSettings[contract.Id] = false;
            AutoFillChanged?.Invoke(contract.Id);
        }

        private void OnContractCompleted(IContract contract)
        {
            _autoFillSettings.Remove(contract.Id);
            AutoFillChanged?.Invoke(contract.Id);
        }

        private void OnContractSkipped(IContract obj)
        {
            _autoFillSettings.Remove(obj.Id);
            AutoFillChanged?.Invoke(obj.Id);
        }
    }
}