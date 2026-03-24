using YeKostenko.ContractClicker.Domain.Boosters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BoosterShopSlotArgs
    {
        public BoosterShopSlot Slot { get; }
        public int SlotIndex { get; }

        public BoosterShopSlotArgs(BoosterShopSlot slot, int slotIndex)
        {
            Slot = slot;
            SlotIndex = slotIndex;
        }
    }
}

