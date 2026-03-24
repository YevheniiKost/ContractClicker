using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.UseCases;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public interface IAutomaticWorkerInfoPresenter
    {
        void AttachView(IAutomaticWorkerInfoView view, int workerId);
        void DetachView();
    }

    public class AutomaticWorkerInfoPresenter : IAutomaticWorkerInfoPresenter
    {
        private readonly IWorkController _workerController;
        private readonly IPlayerWallet _playerWallet;
        private readonly ICalculateNextAutomaticWorkerLevelPriceQuery _calculateNextLevelPriceQuery;

        private IAutomaticWorker _worker;
        private IAutomaticWorkerInfoView _view;

        public AutomaticWorkerInfoPresenter(IWorkController workerController, IPlayerWallet playerWallet,
            ICalculateNextAutomaticWorkerLevelPriceQuery calculateNextLevelPriceQuery)
        {
            _workerController = workerController;
            _playerWallet = playerWallet;
            _calculateNextLevelPriceQuery = calculateNextLevelPriceQuery;
        }

        public void AttachView(IAutomaticWorkerInfoView view, int workerId)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));
            _view.Create += OnCreate;
            _view.UpgradeClick += OnUpgradeClick;
            _view.CloseClick += OnCloseClick;

            _worker = _workerController.AutomaticWorkers.Find(w => w.Id == workerId);
            if (_worker == null)
            {
                throw new System.ArgumentException($"Worker with ID {workerId} not found", nameof(workerId));
            }
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
            _worker = null;
        }

        private void OnCreate()
        {
            UpdateView();
        }

        private void OnUpgradeClick()
        {
            if (_worker == null)
            {
                return;
            }

            int price = _calculateNextLevelPriceQuery.Execute(_worker.Level);
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
            if (_view == null || _worker == null)
            {
                return;
            }

            _view.SetLevel(_worker.Level);
            _view.SetStats(
                yieldPerTick: _worker.GetStat(StatId.YieldPerTick, withLevel: true),
                tickInterval: _worker.GetStat(StatId.TickInterval, withLevel: true),
                critChance: _worker.GetStat(StatId.CritChance, withLevel: true),
                critMultiplier: _worker.GetStat(StatId.CritMultiplier, withLevel: true)
            );

            _view.SetNextLevelStats(
                yieldPerTick: _worker.GetNextLevelStat(StatId.YieldPerTick),
                tickInterval: _worker.GetNextLevelStat(StatId.TickInterval),
                critChance: _worker.GetNextLevelStat(StatId.CritChance),
                critMultiplier: _worker.GetNextLevelStat(StatId.CritMultiplier)
            );

            int price = _calculateNextLevelPriceQuery.Execute(_worker.Level);

            _view.SetUpgradeCost(price);
            bool canAfford = _playerWallet.GetBalance(CurrencyType.Gold) >= price;
            _view.SetUpgradeButtonInteractable(canAfford);
        }
    }
}

