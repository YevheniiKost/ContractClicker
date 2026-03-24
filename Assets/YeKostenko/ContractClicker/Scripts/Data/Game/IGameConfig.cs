namespace YeKostenko.ContractClicker.Data.Game
{
    public interface IGameConfig
    {
        WorkerConfig ManualWorkerConfig { get; }
        WorkerConfig AutoWorkerConfig { get; }
        SlotsConfig ContractSlotsConfig { get; }
        SlotsConfig WorkerSlotsConfig { get; }
        ContractsConfig ContractsConfig { get; }

        int InitialGold { get; }
    }
}