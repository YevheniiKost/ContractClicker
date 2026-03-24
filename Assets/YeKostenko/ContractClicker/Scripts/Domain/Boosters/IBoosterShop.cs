using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Domain.Economy;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public interface IBoosterShop : IDisposable
    {
        event EventHandler ShopRefreshed;
        event EventHandler<BoosterPurchasedEventArgs> BoosterPurchased;

        IReadOnlyList<BoosterShopSlot> CurrentSlots { get; }
        float TimeUntilNextRefreshInSeconds { get; }

        bool TryPurchaseBooster(int slotIndex, IPlayerWallet wallet);
        void ForceRefresh();
    }
}

