using System;

using UnityEngine;

using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public class ManualWorkerInfoPopup : ContractClickerWindow, IManualWorkerInfoView
    {
        [SerializeField]
        private TextView _levelText;
        [SerializeField]
        private TextView _upgradeCostText;
        [SerializeField]
        private WorkerStatsView _currentStatsView;
        [SerializeField]
        private WorkerStatsView _nextLevelStatsView;
        [SerializeField]
        private ButtonView _upgradeButton;
        [SerializeField]
        private ButtonView _closeButton;

        private IManualWorkerInfoPresenter _presenter;

        public event Action Create;
        public event Action UpgradeClick;
        public event Action CloseClick;

        [Inject]
        public void Construct(IManualWorkerInfoPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetUpgradeButtonInteractable(bool interactable)
        {
            _upgradeButton.SetInteractable(interactable);
        }

        public void SetUpgradeCost(int cost)
        {
            _upgradeCostText.SetText(cost.ToString());
        }

        public void SetLevel(int level)
        {
            _levelText.SetText(level.ToString());
        }

        public void SetStats(float clickPower, float critChance, float critMultiplier)
        {
            _currentStatsView.SetYieldPerTickText(clickPower);
            _currentStatsView.SetCritChanceText(critChance);
            _currentStatsView.SetCritMultiplierText(critMultiplier);
        }

        public void SetNextLevelStats(float clickPower, float critChance, float critMultiplier)
        {
            _nextLevelStatsView.SetYieldPerTickText(clickPower);
            _nextLevelStatsView.SetCritChanceText(critChance);
            _nextLevelStatsView.SetCritMultiplierText(critMultiplier);
        }

        public void Close()
        {
           CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            base.OnCreateWindow(context);

            _upgradeButton.ButtonClicked = () => UpgradeClick?.Invoke();
            _closeButton.ButtonClicked = () => CloseClick?.Invoke();

            _presenter.AttachView(this);

            Create?.Invoke();
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            _upgradeButton.ButtonClicked = null;
            _closeButton.ButtonClicked = null;

            _presenter.DetachView();
            _presenter = null;
        }
    }
}