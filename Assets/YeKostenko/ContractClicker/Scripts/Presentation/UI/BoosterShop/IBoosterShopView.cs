using System;
using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IBoosterShopView
    {
        event Action Create;
        event Action<int> SlotClick;

        void SetSlotsData(List<BoosterShopSlotArgs> slots);
        void StartRefreshTimer(float totalSeconds);
        void ShowInsufficientFundsMessage();
        void ShowPurchaseConfirmation(string boosterName, int price, Action onConfirm);
    }
}

