using UnityEngine;

using YeKostenko.CoreKit.App;
using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;
using YevheniiKostenko.CoreKit.Time;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Automation;
using YeKostenko.ContractClicker.Domain.Boosters;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.UseCases;
using YeKostenko.ContractClicker.Domain.Work;
using YeKostenko.ContractClicker.Presentation;
using YeKostenko.ContractClicker.Presentation.UI;
using YeKostenko.ContractClicker.Presentation.UI.WorkerInfo;

namespace YeKostenko.ContractClicker.Application
{
    public class ContractClickerApplication : BaseApp
    {
        private Container _container;

        protected override void OnAppCreate()
        {
            _container = new Container();

            UIRoot.Instance.Initialize(new MonoBehDependencyInjector(_container));
            ITimeProvider timeProvider = UnityTimeProvider.Instance;

            _container.Bind<ITimeProvider>().ToInstance(timeProvider);
            _container.Bind<IGameConfigProvider>().To<GameConfigProvider>().AsSingleton();
            _container.Bind<IWorkController>().To<WorkController>().AsSingleton();
            _container.Bind<IWorkersFactory>().To<WorkersFactory>().AsSingleton();
            _container.Bind<IPlayerWallet>().To<PlayerWallet>().AsSingleton();
            _container.Bind<IContractProcessorModel>().To<ContractProcessorModel>().AsSingleton();
            _container.Bind<IContractGenerator>().To<ContractGenerator>().AsSingleton();
            _container.Bind<IContractFactory>().To<ContractFactory>().AsTransient();
            _container.Bind<IAutoFillModel>().To<AutoFillModel>().AsSingleton();
            _container.Bind<ICalculateNextWorkerPriceQuery>().To<CalculateNextWorkerPriceQuery>().AsTransient();
            _container.Bind<ICalculateNextContractSlotPriceQuery>().To<CalculateNextContractSlotPriceQuery>().AsTransient();
            _container.Bind<ICalculateNextWorkerSlotPriceQuery>().To<CalculateNextWorkerSlotPriceQuery>().AsTransient();
            _container.Bind<ICalculateNextManualWorkerLevelPriceQuery>().To<CalculateNextManualWorkerLevelPriceQuery>().AsTransient();
            _container.Bind<ICalculateNextAutomaticWorkerLevelPriceQuery>().To<CalculateNextAutomaticWorkerLevelPriceQuery>()
                .AsTransient();
            _container.Bind<IContractModifierService>().To<ContractModifierService>().AsTransient();

            _container.Bind<IApplyBoosterUseCase>().To<ApplyBoosterUseCase>().AsTransient();

            BoosterShopConfig boosterShopConfig = Resources.Load<BoosterShopConfig>("BoosterShopConfig");
            if (boosterShopConfig != null)
            {
                IBoosterManager boosterManager = new BoosterManager(timeProvider);
                IBoosterShop boosterShop = new BoosterShop(boosterShopConfig, timeProvider);

                _container.Bind<IBoosterManager>().ToInstance(boosterManager);
                _container.Bind<IBoosterShop>().ToInstance(boosterShop);
            }

            _container.Bind<IMessageBoxPresenter>().To<MessageBoxPresenter>().AsTransient();

            _container.Bind<IContractClickerNavigation>().ToInstance(new ContractClickerNavigation(UIRoot.Instance.UIManager));
            _container.Bind<IWorkButtonPresenter>().To<WorkButtonPresenter>().AsTransient();
            _container.Bind<ILauncherPresenter>().To<LauncherPresenter>().AsTransient();
            _container.Bind<IContractPanelPresenter>().To<ContractPanelPresenter>().AsTransient();
            _container.Bind<IPlayerProgressPanelPresenter>().To<PlayerProgressPanelPresenter>().AsTransient();
            _container.Bind<IWorkersPanelPresenter>().To<WorkersPanelPresenter>().AsTransient();
            _container.Bind<IContractSelectionPresenter>().To<ContractSelectionPresenter>().AsTransient();
            _container.Bind<IContractCompletedPresenter>().To<ContractCompletedPresenter>().AsTransient();
            _container.Bind<IBuyWorkerPresenter>().To<BuyWorkerPresenter>().AsTransient();
            _container.Bind<IBuySlotPresenter>().To<BuySlotPresenter>().AsTransient();
            _container.Bind<IManualWorkerInfoPresenter>().To<ManualWorkerInfoPresenter>().AsTransient();
            _container.Bind<IAutomaticWorkerInfoPresenter>().To<AutomaticWorkerInfoPresenter>().AsTransient();
            _container.Bind<IBoosterShopPresenter>().To<BoosterShopPresenter>().AsTransient();
        }

        protected override void OnAppStart()
        {
            _container.Resolve<IContractClickerNavigation>().OpenLauncher();
        }

        protected override void OnAppDestroy()
        {

        }
    }
}