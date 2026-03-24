namespace YeKostenko.ContractClicker.Data.Game
{
    public class SlotsConfig
    {
        public int InitialSlots { get; }
        public int MaxSlots { get; }
        public int BasePrice { get; }
        public float PriceMultiplier { get; }

        public SlotsConfig(int initialSlots, int maxSlots, int basePrice, float priceMultiplier)
        {
            InitialSlots = initialSlots;
            MaxSlots = maxSlots;
            BasePrice = basePrice;
            PriceMultiplier = priceMultiplier;
        }
    }
}