using System;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterPurchasedEventArgs : EventArgs
    {
        public BoosterPurchasedEventArgs(BoosterShopSlot slot, int slotIndex)
        {
            Slot = slot;
            SlotIndex = slotIndex;
        }

        public BoosterShopSlot Slot { get; }
        public int SlotIndex { get; }
    }
}

