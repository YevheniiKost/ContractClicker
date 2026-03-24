using YeKostenko.ContractClicker.Domain.Contract;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractSlotArgs
    {
        public int Id { get; }
        public IContract Contract { get; private set; }
        public bool IsAutoFill { get; }

        public bool IsLocked { get; private set; }
        public bool IsEmpty => Contract == null;

        private ContractSlotArgs(IContract contract, bool isLocked, bool isAutoFill, int id)
        {
            Contract = contract;
            IsLocked = isLocked;
            IsAutoFill = isAutoFill;
            Id = id;
        }

        public static ContractSlotArgs Empty(int slotId, bool isLocked)
        {
            return new ContractSlotArgs(null, isLocked, false, slotId);
        }

        public static ContractSlotArgs WithContract(int slotId, IContract contract, bool isAutoFill)
        {
            return new ContractSlotArgs(contract, false, isAutoFill, slotId);
        }
    }
}