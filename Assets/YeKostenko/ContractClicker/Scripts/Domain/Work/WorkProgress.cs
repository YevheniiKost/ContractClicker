using System;

using YeKostenko.ContractClicker.Data;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public class WorkProgress
    {
        public WorkProgress(float amount, ProgressType type)
        {
            Amount = amount;
            Type = type;
        }

        public event Action ProgressChanged;

        public float Amount { get; private set; }
        public ProgressType Type { get; }

        public void AddProgress(float amount)
        {
            if (amount < 0)
            {
                return;
            }

            Amount += amount;
            ProgressChanged?.Invoke();
        }

        public void SpendProgress(float amount)
        {
            if (amount < 0 || amount > Amount)
            {
                return;
            }

            Amount -= amount;
            ProgressChanged?.Invoke();
        }
    }
}