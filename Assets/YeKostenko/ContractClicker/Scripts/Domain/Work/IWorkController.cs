using System;
using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public interface IWorkController : IDisposable
    {
        event Action AutomaticWorkersChanged;

        WorkProgress ManualProgress { get; }
        WorkProgress AutomaticProgress { get; }

        int MaxWorkerSlots { get; }
        int UnlockedWorkerSlots { get; }

        IManualWorker ManualWorker { get; }
        List<IAutomaticWorker> AutomaticWorkers { get; }

        void Initialize(IManualWorker manualWorker, List<IAutomaticWorker> automaticWorkers);
        void AddAutomaticWorker(IAutomaticWorker automaticWorker);
        void OpenNewContractSlot();
    }
}