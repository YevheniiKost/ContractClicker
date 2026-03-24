using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.UseCases;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public class ManualWorkerInfoPresenter : IManualWorkerInfoPresenter
    {
        private readonly IWorkController _workerController;
        private readonly IPlayerWallet _playerWallet;
        private readonly ICalculateNextManualWorkerLevelPriceQuery _calculateNextLevelPriceQuery;

        private IManualWorker _worker;
        private IManualWorkerInfoView _view;

        public ManualWorkerInfoPresenter(IWorkController workerController, IPlayerWallet playerWallet,
            ICalculateNextManualWorkerLevelPriceQuery calculateNextLevelPriceQuery)
        {
            _workerController = workerController;
            _playerWallet = playerWallet;
            _calculateNextLevelPriceQuery = calculateNextLevelPriceQuery;
        }

        public void AttachView(IManualWorkerInfoView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));
            _view.Create += OnCreate;
            _view.UpgradeClick += OnUpgradeClick;
            _view.CloseClick += OnCloseClick;
        }

        public void DetachView()
        {
            if (_view == null)
            {
                return;
            }

            _view.Create -= OnCreate;
            _view.UpgradeClick -= OnUpgradeClick;
            _view.CloseClick -= OnCloseClick;
            _view = null;
        }

        private void OnCreate()
        {
            _worker = _workerController.ManualWorker;
            UpdateView();
        }

        private void OnUpgradeClick()
        {
            if (_worker == null)
            {
                return;
            }

            int price = _calculateNextLevelPriceQuery.Execute();
            if (_playerWallet.TrySpendFunds(CurrencyType.Gold, price))
            {
                _worker.LevelUpWorker();
                UpdateView();
            }
        }

        private void OnCloseClick()
        {
            _view.Close();
        }

        private void UpdateView()
        {
            if (_view == null)
            {
                return;
            }

            _view.SetLevel(_worker.Level);
            _view.SetStats(
                clickPower: _worker.GetStat(StatId.ClickPower, withLevel: true),
                critChance: _worker.GetStat(StatId.CritChance, withLevel: true),
                critMultiplier: _worker.GetStat(StatId.CritMultiplier, withLevel: true)
            );

            _view.SetNextLevelStats(
                clickPower: _worker.GetNextLevelStat(StatId.ClickPower),
                critChance: _worker.GetNextLevelStat(StatId.CritChance),
                critMultiplier: _worker.GetNextLevelStat(StatId.CritMultiplier)
            );

            int price = _calculateNextLevelPriceQuery.Execute();

            _view.SetUpgradeCost(price);
            bool canAfford = _playerWallet.GetBalance(CurrencyType.Gold) >= price;
            _view.SetUpgradeButtonInteractable(canAfford);
        }
    }
}