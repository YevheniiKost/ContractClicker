using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Data.Game
{
    public class ContractDefinition
    {
        public readonly int Id;
        public readonly string Name;
        public readonly float RequiredProgress;
        public readonly int Reward;

        public readonly ModifierSetConfig ModifiersSet;

        public ContractDefinition(int id, string name, float requiredProgress, int reward, ModifierSetConfig modifiersSet)
        {
            Id = id;
            Name = name;
            RequiredProgress = requiredProgress;
            Reward = reward;
            ModifiersSet = modifiersSet;
        }
    }
}