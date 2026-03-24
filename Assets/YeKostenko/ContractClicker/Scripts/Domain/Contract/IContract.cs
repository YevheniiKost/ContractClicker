using System.Collections.Generic;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public interface IContract
    {
        int Id { get; }
        string Name { get; }

        float RequiredProgress { get; }
        float CurrentProgress { get; }

        int Reward { get; }
        int FinalReward { get; } // Винагорода з урахуванням модифікаторів
        bool IsCompleted { get; }

        IReadOnlyList<IContractModifier> Modifiers { get; }

        void AddProgress(float amount, ProgressType progressType);

        // Методи для роботи з модифікаторами
        void AddModifier(IContractModifier modifier);
        void RemoveModifier(IContractModifier modifier);
        void RemoveModifier(string modifierId);
        bool HasModifier<T>() where T : IContractModifier;
        T GetModifier<T>() where T : IContractModifier;

        // Оновлення модифікаторів (для часових)
        void UpdateModifiers(float deltaTime);
    }
}