using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuyWorkerPopUpUIContext : IUIContext
    {
        public readonly Action OnClose;

        public BuyWorkerPopUpUIContext(Action onClose)
        {
            OnClose = onClose;
        }
    }
}