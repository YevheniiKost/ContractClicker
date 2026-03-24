using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Domain.UseCases
{
    public class CalculateNextAutomaticWorkerLevelPriceQuery : ICalculateNextAutomaticWorkerLevelPriceQuery
    {
        private readonly IGameConfigProvider _gameConfigProvider;

        public CalculateNextAutomaticWorkerLevelPriceQuery(IGameConfigProvider gameConfigProvider)
        {
            _gameConfigProvider = gameConfigProvider;
        }

        public int Execute(int currentLevel)
        {
            IGameConfig config = _gameConfigProvider.GetGameConfig();
            int basePrice = config.AutoWorkerConfig.InitialUpgradePrice;
            float priceMultiplier = config.AutoWorkerConfig.UpgradePriceMultiplier;

            int nextLevel = currentLevel + 1;
            int price = (int)(basePrice * System.Math.Pow(priceMultiplier, nextLevel - 1));

            return price;
        }
    }
}



