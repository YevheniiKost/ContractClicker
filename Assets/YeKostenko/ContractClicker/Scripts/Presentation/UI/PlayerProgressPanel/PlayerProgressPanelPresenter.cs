using System;

using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class PlayerProgressPanelPresenter : IPlayerProgressPanelPresenter
    {
        private readonly IPlayerWallet _playerWallet;
        private readonly IWorkController _workController;

        private IPlayerProgressPanelView _view;

        public PlayerProgressPanelPresenter(IPlayerWallet playerWallet,
            IWorkController workController)
        {
            _playerWallet = playerWallet;
            _workController = workController;
        }

        public void AttachView(IPlayerProgressPanelView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.Create += OnCreate;

            _playerWallet.BalanceChanged += OnBalanceChanged;
            _workController.ManualProgress.ProgressChanged += OnWorkProgressChanged;
            _workController.AutomaticProgress.ProgressChanged += OnWorkProgressChanged;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnCreate;
            }

            _playerWallet.BalanceChanged -= OnBalanceChanged;
            _workController.ManualProgress.ProgressChanged -= OnWorkProgressChanged;
            _workController.AutomaticProgress.ProgressChanged -= OnWorkProgressChanged;

            _view = null;
        }

        private void OnCreate()
        {
            _view.SetPlayerGold(_playerWallet.GetBalance(CurrencyType.Gold));
            UpdateWorkProgress();
        }

        private void OnBalanceChanged(CurrencyType currencyType, int newBalance)
        {
            if (currencyType == CurrencyType.Gold)
            {
                _view.SetPlayerGold(newBalance);
            }
        }

        private void OnWorkProgressChanged()
        {
            UpdateWorkProgress();
        }

        private void UpdateWorkProgress()
        {
            float manualProgress = _workController.ManualProgress.Amount;
            float autoProgress = _workController.AutomaticProgress.Amount;

            _view.SetWorkProgress(manualProgress, autoProgress);
        }
    }
}