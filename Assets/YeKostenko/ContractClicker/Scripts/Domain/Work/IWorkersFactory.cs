﻿using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public interface IWorkersFactory
    {
        IManualWorker CreateManualWorker(ManualWorkerStats stats, int level, WorkerConfig config);
        IAutomaticWorker CreateAutomaticWorker(int id, AutomaticWorkerStats stats, int level, WorkerConfig config);
    }
}