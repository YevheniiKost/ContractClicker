using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractSelectionView
    {
        event Action Create;
        event Action CloseClick;
        event Action<int> ContractSelected;

        void SetContracts(List<ContractDefinition> contracts);
        void Close(ContractDefinition contractDefinition);
    }
}