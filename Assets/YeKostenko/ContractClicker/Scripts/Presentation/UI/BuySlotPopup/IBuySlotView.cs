using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IBuySlotView
    {
        event Action<BuySlotPopupType> Create;
        event Action BuyButtonClick;
        event Action CloseButtonClick;

        void SetTitleText(string text);
        void SetPrice(string text);
        void SetBuyButtonInteractable(bool interactable);
        void Close();
    }
}