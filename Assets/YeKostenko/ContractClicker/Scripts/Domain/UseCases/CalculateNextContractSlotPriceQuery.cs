using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Contract;

namespace YeKostenko.ContractClicker.Domain.UseCases
{
    public class CalculateNextContractSlotPriceQuery : ICalculateNextContractSlotPriceQuery
    {
        private readonly IContractProcessorModel _contractProcessorModel;
        private readonly IGameConfigProvider _gameConfigProvider;

        public CalculateNextContractSlotPriceQuery(IContractProcessorModel contractProcessorModel,
            IGameConfigProvider gameConfigProvider)
        {
            _contractProcessorModel = contractProcessorModel;
            _gameConfigProvider = gameConfigProvider;
        }

        public int Execute()
        {
            IGameConfig config = _gameConfigProvider.GetGameConfig();
            int basePrice = config.ContractSlotsConfig.BasePrice;
            float priceMultiplier = config.ContractSlotsConfig.PriceMultiplier;
            int maxSlots = config.ContractSlotsConfig.MaxSlots;

            int nextSlotIndex = maxSlots - _contractProcessorModel.UnlockedContractSlots;
            return basePrice * (int)Mathf.Pow(priceMultiplier, maxSlots - nextSlotIndex);
        }
    }
}