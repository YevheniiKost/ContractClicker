using System;

using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IBuyWorkerView
    {
        event Action Create;
        event Action BuyButtonClick;
        event Action CloseButtonClick;

        void SetWorkerData(AutomaticWorkerStats stats);
        void SetPrice(int price);
        void SetBuyButtonInteractable(bool interactable);
        void Close();
    }
}