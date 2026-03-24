﻿using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.UseCases;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuyWorkerPresenter : IBuyWorkerPresenter
    {
        private readonly IPlayerWallet _wallet;
        private readonly IWorkController _workController;
        private readonly ICalculateNextWorkerPriceQuery _calculateNextWorkerPriceQuery;
        private readonly IGameConfigProvider _gameConfigProvider;
        private readonly IWorkersFactory _workersFactory;

        private IBuyWorkerView _view;

        public BuyWorkerPresenter(IPlayerWallet wallet,
            IWorkController workController,
            ICalculateNextWorkerPriceQuery calculateNextWorkerPriceQuery,
            IGameConfigProvider gameConfigProvider,
            IWorkersFactory workersFactory)
        {
            _wallet = wallet;
            _workController = workController;
            _calculateNextWorkerPriceQuery = calculateNextWorkerPriceQuery;
            _gameConfigProvider = gameConfigProvider;
            _workersFactory = workersFactory;
        }

        public void AttachView(IBuyWorkerView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.BuyButtonClick += OnBuyButtonClick;
            _view.CloseButtonClick += OnCloseButtonClick;
        }

        public void DetachView()
        {
            _view.Create -= OnCreate;
            _view.BuyButtonClick -= OnBuyButtonClick;
            _view.CloseButtonClick -= OnCloseButtonClick;

            _view = null;
        }

        private void OnCreate()
        {
            int nextWorkerPrice = _calculateNextWorkerPriceQuery.Execute();
            AutomaticWorkerStats workerStats = GetWorkerStats();

            _view.SetWorkerData(workerStats);
            _view.SetPrice(nextWorkerPrice);
            ICurrency currency = _wallet.GetCurrency(CurrencyType.Gold);
            _view.SetBuyButtonInteractable(currency.Amount >= nextWorkerPrice);
        }

        private AutomaticWorkerStats GetWorkerStats()
        {
            IGameConfig gameConfig = _gameConfigProvider.GetGameConfig();
            WorkerConfig workerConfig = gameConfig.AutoWorkerConfig;

            return new AutomaticWorkerStats(workerConfig.CriticalChance,
                workerConfig.CriticalMultiplier,
                workerConfig.LevelLinearProgressionFactor,
                workerConfig.PowerCurveProgressionFactor,
                workerConfig.WorkPower,
                workerConfig.WorkPauseDuration);
        }

        private void OnBuyButtonClick()
        {
            int nextWorkerPrice = _calculateNextWorkerPriceQuery.Execute();
            if (_wallet.TrySpendFunds(CurrencyType.Gold, nextWorkerPrice))
            {
                IGameConfig gameConfig = _gameConfigProvider.GetGameConfig();
                WorkerConfig workerConfig = gameConfig.AutoWorkerConfig;

                IAutomaticWorker worker = _workersFactory.CreateAutomaticWorker(_workController.AutomaticWorkers.Count,
                    GetWorkerStats(), 1, workerConfig);
                _workController.AddAutomaticWorker(worker);
                _view.Close();
            }
        }

        private void OnCloseButtonClick()
        {
            _view.Close();
        }
    }
}