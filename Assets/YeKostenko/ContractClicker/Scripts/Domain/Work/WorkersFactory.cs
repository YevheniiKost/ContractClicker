using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public class WorkersFactory : IWorkersFactory
    {
        public static IWorkersFactory Default { get; } = new WorkersFactory();

        public IManualWorker CreateManualWorker(ManualWorkerStats stats, int level, WorkerConfig config) =>
            new ManualWorker(stats, level, config, new WorkerEffectsController());

        public IAutomaticWorker CreateAutomaticWorker(int id, AutomaticWorkerStats stats, int level, WorkerConfig config) =>
            new AutomaticWorker(id, stats, level, config, new WorkerEffectsController());
    }
}