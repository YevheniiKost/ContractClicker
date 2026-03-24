using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    public interface IContractModifierService
    {
        ModifierSetConfig ApplyRandomModifiersSet(IContract contract);
        ModifierSetConfig GetRandomModifiersSet();
        ModifierSetConfig GetRandomModifiersSet(ContractDifficulty difficulty);
    }
}