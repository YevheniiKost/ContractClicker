using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Domain.UseCases
{
    public class CalculateNextWorkerSlotPriceQuery : ICalculateNextWorkerSlotPriceQuery
    {
        private readonly IWorkController _workController;
        private readonly IGameConfigProvider _configProvider;

        public CalculateNextWorkerSlotPriceQuery(IWorkController workController, IGameConfigProvider configProvider)
        {
            _workController = workController;
            _configProvider = configProvider;
        }

        public int Execute()
        {
            IGameConfig config = _configProvider.GetGameConfig();
            int basePrice = config.WorkerSlotsConfig.BasePrice;
            float priceMultiplier = config.WorkerSlotsConfig.PriceMultiplier;
            int maxSlots = config.WorkerSlotsConfig.MaxSlots;

            int slotsPurchased = maxSlots - _workController.UnlockedWorkerSlots;

            return basePrice * (int)Mathf.Pow(priceMultiplier, maxSlots - slotsPurchased);
        }
    }
}