namespace YeKostenko.ContractClicker.Domain.Work
{
    public interface IManualWorker : IWorker
    {
        void ProcessClick();
    }
}