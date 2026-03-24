namespace YeKostenko.ContractClicker.Domain.Work
{
    public interface IAutomaticWorker : IWorker
    {
        int Id { get; }
        AutomaticWorkerStats Stats { get; }
    }
}