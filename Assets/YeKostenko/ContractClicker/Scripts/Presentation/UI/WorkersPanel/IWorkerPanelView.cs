using System;
using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IWorkersPanelView
    {
        event Action Create;
        event Action<int> WorkerSlotClick;

        void SetWorkersData(List<WorkerSlotArgs> slots);
        void OpenBuySlotPopup(Action onClose);
        void OpenBuyWorkerPopup(Action onClose);
        void OpenWorkerInfoPopup(int id, Action onClose);
    }
}