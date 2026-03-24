using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Domain.UseCases
{
    public class CalculateNextManualWorkerLevelPriceQuery : ICalculateNextManualWorkerLevelPriceQuery
    {
        private readonly IWorkController _workController;
        private readonly IGameConfigProvider _configProvider;

        public CalculateNextManualWorkerLevelPriceQuery(IWorkController workController, IGameConfigProvider configProvider)
        {
            _workController = workController;
            _configProvider = configProvider;
        }

        public int Execute()
        {
            IGameConfig config = _configProvider.GetGameConfig();
            int initialPrice = config.ManualWorkerConfig.InitialUpgradePrice;
            float priceMultiplier = config.ManualWorkerConfig.UpgradePriceMultiplier;
            IManualWorker manualWorker = _workController.ManualWorker;
            int nextLevel = manualWorker.Level + 1;

            int price = (int)(initialPrice * System.Math.Pow(priceMultiplier, nextLevel - 1));

            return price;
        }
    }
}