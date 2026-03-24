using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class WorkerSlotArgs
    {
        public int SlotIndex { get; }
        public IAutomaticWorker Worker { get; }
        public bool IsLocked { get; }
        public bool IsEmpty => Worker == null;

        private WorkerSlotArgs(int slotIndex, IAutomaticWorker worker, bool isLocked)
        {
            SlotIndex = slotIndex;
            Worker = worker;
            IsLocked = isLocked;
        }
        
        public static WorkerSlotArgs Empty(int slotIndex, bool isLocked)
        {
            return new WorkerSlotArgs(slotIndex, null, isLocked);
        }
         
        public static WorkerSlotArgs WithWorker(int slotIndex, IAutomaticWorker worker)
        {
            return new WorkerSlotArgs(slotIndex, worker, false);
        }
    }
}