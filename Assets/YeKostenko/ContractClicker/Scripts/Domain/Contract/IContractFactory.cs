using YeKostenko.ContractClicker.Data.Game;

using YevheniiKostenko.CoreKit.Utils;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public interface IContractFactory : IFactory<IContract, ContractDefinition>
    {
    }
}