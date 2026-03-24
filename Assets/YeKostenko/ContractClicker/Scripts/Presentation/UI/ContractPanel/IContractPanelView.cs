using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractPanelView
    {
        event Action Create;
        event Action<int> SlotClicked;
        event Action<int> FillContractClicked;
        event Action<int, bool> AutoFillToggled;

        void SetContractsData(List<ContractSlotArgs> contracts);
        void OpenContractSelectionPopUp(Action<ContractDefinition> onSelect);
        void OpenContractCompletePopUp(int contractId, Action onClose);
        void OpenContractInfoPopUp(int contractId, Action onClose);
        void OpenFillContractWidget(int slotId, float progressToShow, Action<float> addedProgress);
        void OpenBuyContractSlotPopup(Action onClose);
    }
}