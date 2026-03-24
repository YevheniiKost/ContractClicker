using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public class AutomaticWorkerInfoUIContext : IUIContext
    {
        public int WorkerId { get; }
        public Action OnClose { get; }

        public AutomaticWorkerInfoUIContext(int workerId, Action onClose)
        {
            WorkerId = workerId;
            OnClose = onClose;
        }
    }
}

