using System;

using Cysharp.Threading.Tasks;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Presentation;
using YeKostenko.ContractClicker.Presentation.UI;
using YeKostenko.ContractClicker.Presentation.UI.WorkerInfo;
using YeKostenko.CoreKit.UI;

using Logger = YeKostenko.CoreKit.Logging.Logger;

namespace YeKostenko.ContractClicker.Application
{
    public class ContractClickerNavigation : IContractClickerNavigation
    {
        private readonly UIManager _uiManager;

        public ContractClickerNavigation(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void OpenMainMenu()
        {
            _uiManager.CloseAllAsync().Forget();
            _uiManager.OpenWindowAsync<MainMenuWindow>().Forget();
        }

        public void OpenLauncher()
        {
            _uiManager.OpenWindowAsync<LauncherWindow>().Forget();
        }

        public void OpenBoosterShop()
        {
            _uiManager.OpenWindowAsync<BoosterShopWindow>().Forget();
        }

        public void OpenMessageBox(string message, string title = null, string yesButtonText = null,
            string noButtonText = null, Action onYes = null, Action onNo = null)
        {
            MessageBoxUIContext context = new MessageBoxUIContext(message, title, yesButtonText, noButtonText, onYes, onNo);
            _uiManager.OpenWindowAsync<MessageBoxWindow>(context).Forget();
        }

        public void OpenManualWorkerInfoPopUp()
        {
            _uiManager.OpenWindowAsync<ManualWorkerInfoPopup>().Forget();
        }

        public void OpenContractSelectionPopUp(Action<ContractDefinition> onSelect)
        {
            _uiManager.OpenWindowAsync<ContractSelectionPopUp>(new ContractSelectionUIContext(onSelect)).Forget();
        }

        public void OpenContractCompletePopUp(int contractId, Action onClose)
        {
            Logger.Log($"Opening Contract Complete PopUp for contractId: {contractId}");
            _uiManager.OpenWindowAsync<ContractCompletedPopup>(new ContractCompletedUIContext(contractId, onClose)).Forget();
        }

        public void OpenContractInfoPopUp(int contractId, Action onClose)
        {
            Logger.Log($"Opening Contract Info PopUp for contractId: {contractId}");
            // _uiManager.OpenWindowAsync<ContractInfoPopUp>(contractId).Forget();
        }

        public void OpenBuyWorkerSlotPopup(Action onClose)
        {
            _uiManager.OpenWindowAsync<BuySlotPopup>(new BuySlotUIContext(BuySlotPopupType.Worker, onClose)).Forget();
        }

        public void OpenBuyContractSlotPopup(Action onClose)
        {
            _uiManager.OpenWindowAsync<BuySlotPopup>(new BuySlotUIContext(BuySlotPopupType.Contract, onClose)).Forget();
        }

        public void OpenBuyWorkerPopup(Action onClose)
        {
            Logger.Log("Opening Buy Worker PopUp");
             _uiManager.OpenWindowAsync<BuyWorkerPopUp>(new BuyWorkerPopUpUIContext(onClose)).Forget();
        }

        public void OpenWorkerInfoPopup(int id, Action onClose)
        {
            _uiManager.OpenWindowAsync<AutomaticWorkerInfoPopup>(new AutomaticWorkerInfoUIContext(id, onClose)).Forget();
        }
    }
}