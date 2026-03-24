using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuySlotUIContext : IUIContext
    {
        public BuySlotPopupType PopupType { get; }
        public Action OnClose { get; }

        public BuySlotUIContext(BuySlotPopupType popupType, Action onClose)
        {
            PopupType = popupType;
            OnClose = onClose;
        }
    }
}