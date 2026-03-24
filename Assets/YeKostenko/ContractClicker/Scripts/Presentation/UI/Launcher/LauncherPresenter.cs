﻿using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Automation;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class LauncherPresenter : ILauncherPresenter
    {
        private readonly IWorkController _workController;
        private readonly IWorkersFactory _workersFactory;
        private readonly IGameConfigProvider _gameConfigProvider;
        private readonly IPlayerWallet _playerWallet;
        private readonly IAutoFillModel _autoFillModel;
        private readonly IContractProcessorModel _contractProcessorModel;

        private ILauncherView _view;

        public LauncherPresenter(IWorkController workController, IWorkersFactory workersFactory,
            IGameConfigProvider gameConfigProvider, IPlayerWallet playerWallet, IAutoFillModel autoFillModel, IContractProcessorModel contractProcessorModel)
        {
            _workController = workController;
            _workersFactory = workersFactory;
            _gameConfigProvider = gameConfigProvider;
            _playerWallet = playerWallet;
            _autoFillModel = autoFillModel;
            _contractProcessorModel = contractProcessorModel;
        }

        public void AttachView(ILauncherView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
        }

        public void DetachView()
        {
            if (_view == null)
            {
                return;
            }

            _view.Create -= OnCreate;
            _view = null;
        }

        private void OnCreate()
        {
            InitializeApp().Forget();
        }

        private async UniTask InitializeApp()
        {
            //todo:
            // - authentication
            // - load user data
            // - initialize other systems
            SetProgress(0);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            SetProgress(0.2f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            SetProgress(0.4f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            SetProgress(0.6f);

            await _gameConfigProvider.Initialize();

            IGameConfig gameConfig = _gameConfigProvider.GetGameConfig();

            //todo: pass config to other systems

            IManualWorker manualWorker = _workersFactory.CreateManualWorker(new ManualWorkerStats(
                gameConfig.ManualWorkerConfig.CriticalChance,
                gameConfig.ManualWorkerConfig.CriticalMultiplier,
                gameConfig.ManualWorkerConfig.LevelLinearProgressionFactor,
                gameConfig.ManualWorkerConfig.PowerCurveProgressionFactor,
                gameConfig.ManualWorkerConfig.WorkPower,
                gameConfig.ManualWorkerConfig.WorkPauseDuration), 1, gameConfig.ManualWorkerConfig);

            _workController.Initialize(manualWorker, new List<IAutomaticWorker>());
            _contractProcessorModel.Initialize();

            _playerWallet.Initialize(new List<Currency>
            {
                new Currency("gold", "Gold", CurrencyType.Gold, gameConfig.InitialGold)
            });

            _autoFillModel.Initialize();

            _view.OpenMainMenu();
        }

        private void SetProgress(float progress)
        {
            progress = Math.Clamp(progress, 0f, 1f);
            _view.SetLoadingProgress(progress);
        }
    }
}