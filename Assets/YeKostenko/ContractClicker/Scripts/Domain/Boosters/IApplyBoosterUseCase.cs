using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public interface IApplyBoosterUseCase
    {
        void Execute(BoosterConfig config);
    }
}

