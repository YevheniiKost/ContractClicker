using System;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Presentation
{
    public interface IContractClickerNavigation
    {
        void OpenMainMenu();
        void OpenLauncher();
        void OpenBoosterShop();
        void OpenMessageBox(string message, string title = null, string yesButtonText = null,
            string noButtonText = null, Action onYes = null, Action onNo = null);
        void OpenManualWorkerInfoPopUp();
        void OpenContractSelectionPopUp(Action<ContractDefinition> onSelect);
        void OpenContractCompletePopUp(int contractId, Action onClose);
        void OpenContractInfoPopUp(int contractId, Action onClose);
        void OpenBuyWorkerSlotPopup(Action onClose);
        void OpenBuyWorkerPopup(Action onClose);
        void OpenWorkerInfoPopup(int id, Action onClose);
        void OpenBuyContractSlotPopup(Action onClose);
    }
}