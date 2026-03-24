using System;

using YeKostenko.ContractClicker.Domain.Economy;

namespace YeKostenko.ContractClicker.Domain.Automation
{
    public interface IAutoFillModel : IDisposable
    {
        event Action<int> AutoFillChanged;

        void Initialize();
        void SetContractAutoFill(int contractId, bool isEnabled);
        bool IsContractAutoFillEnabled(int contractId);
    }
}