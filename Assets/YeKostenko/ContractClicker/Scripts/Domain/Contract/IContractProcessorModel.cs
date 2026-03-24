using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public interface IContractProcessorModel
    {
        event Action<IContract> NewContractStarted;
        event Action<IContract> ContractCompleted;
        event Action<IContract> ProgressAdded;
        event Action<IContract> ContractSkipped;

        List<IContract> ActiveContracts { get; }
        int UnlockedContractSlots { get; }
        int MaxContractSlots { get; }

        //todo add saved contracts
        void Initialize();

        bool StartNewContract(IContract contract);

        void AddProgressToContract(int contractId, float amount, ProgressType progressType);
        void ClearCompletedContracts();
        void ClearContract(int contractId);
        void SkipContract(int contractId);

        void OpenNewContractSlot();
    }
}