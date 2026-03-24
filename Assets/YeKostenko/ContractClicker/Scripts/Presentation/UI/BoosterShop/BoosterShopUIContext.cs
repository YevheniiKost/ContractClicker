using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BoosterShopUIContext : IUIContext
    {
        public Action OnClose { get; }

        public BoosterShopUIContext(Action onClose = null)
        {
            OnClose = onClose;
        }
    }
}

