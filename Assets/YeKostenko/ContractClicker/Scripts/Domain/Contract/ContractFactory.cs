using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public class ContractFactory : IContractFactory
    {
        private readonly ModifierFactory _modifierFactory = ModifierFactory.Default;

        public static IContractFactory Default { get; } = new ContractFactory();

        public IContract Create(ContractDefinition param)
        {
            Contract contract = new Contract(param.Id, param.Name, param.RequiredProgress, param.Reward);

            if (param.ModifiersSet != null)
            {
                foreach (IContractModifier modifier in _modifierFactory.CreateFromSet(param.ModifiersSet))
                {
                    contract.AddModifier(modifier);
                }
            }

            return contract;
        }
    }
}