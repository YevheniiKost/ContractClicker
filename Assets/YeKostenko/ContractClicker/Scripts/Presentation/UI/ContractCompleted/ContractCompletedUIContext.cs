using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractCompletedUIContext : IUIContext
    {
        public int ContractId { get; }
        public Action CloseCallback { get; }

        public ContractCompletedUIContext(int contractId, Action closeCallback)
        {
            ContractId = contractId;
            CloseCallback = closeCallback;
        }
    }
}