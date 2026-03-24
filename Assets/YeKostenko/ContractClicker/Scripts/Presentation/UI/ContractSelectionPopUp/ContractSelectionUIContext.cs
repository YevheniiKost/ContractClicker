using System;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public struct ContractSelectionUIContext : IUIContext
    {
        public readonly Action<ContractDefinition> OnContractSelected;

        public ContractSelectionUIContext(Action<ContractDefinition> onContractSelected)
        {
            OnContractSelected = onContractSelected;
        }
    }
}