using UnityEngine;
using UnityEngine.UI;

using YeKostenko.CoreKit.DI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class MainMenuWindow : ContractClickerWindow, IMainMenuView
    {
        [Header("Components")]
        [SerializeField]
        private WorkButtonView _workButtonView;
        [SerializeField]
        private PlayerProgressPanelView _playerProgressPanelView;
        [SerializeField]
        private ContractPanelView _contractPanelView;
        [SerializeField]
        private WorkersPanelView _workersPanelView;
        [SerializeField]
        private ButtonView _boosterShopButton;

        [Inject]
        public void Construct(IContractClickerNavigation navigation,
            IContractPanelPresenter contractPanelPresenter,
            IPlayerProgressPanelPresenter playerProgressPanelPresenter,
            IWorkButtonPresenter workButtonPresenter,
            IWorkersPanelPresenter workersPanelPresenter)
        {
            _workButtonView.Initialize(workButtonPresenter, navigation);
            _playerProgressPanelView.Initialize(playerProgressPanelPresenter);
            _contractPanelView.Initialize(navigation, contractPanelPresenter);
            _workersPanelView.Initialize(workersPanelPresenter, navigation);
            _boosterShopButton.ButtonClicked = () => navigation.OpenBoosterShop();
        }
    }
}