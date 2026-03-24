using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    public interface IModifierDatabase
    {
        ModifierSetConfig GetRandomSet(int contractLevel);
        ModifierSetConfig GetRandomSet(ContractDifficulty difficulty, int contractLevel);

        float BaseModifierChance { get; }
        IReadOnlyList<ModifierSetConfig> ModifierSets { get; }
    }
}