using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterShopSlot
    {
        public BoosterShopSlot(BoosterConfig config)
        {
            Config = config;
            IsPurchased = false;
        }

        public BoosterConfig Config { get; }
        public bool IsPurchased { get; private set; }

        public void MarkAsPurchased()
        {
            IsPurchased = true;
        }
    }
}

