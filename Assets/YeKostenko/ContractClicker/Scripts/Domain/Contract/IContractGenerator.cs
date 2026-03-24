using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public interface IContractGenerator
    {
        List<ContractDefinition> GenerateContracts(int count);
    }
}