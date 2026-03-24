using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    public class ContractModifierService : IContractModifierService
    {
        private readonly IGameConfigProvider _configProvider;
        private readonly ModifierFactory _factory;

        private IModifierDatabase _database;

        public ContractModifierService(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _factory = ModifierFactory.Default;
        }

        public ModifierSetConfig ApplyRandomModifiersSet(IContract contract)
        {
            _database = _configProvider.GetModifierDatabase();

            if (Random.Range(0f, 100f) > _database.BaseModifierChance)
            {
                return null;
            }

            ModifierSetConfig set = _database.GetRandomSet(999);

            ApplyModifierSet(contract, set);

            return set;
        }

        public ModifierSetConfig GetRandomModifiersSet()
        {
            _database = _configProvider.GetModifierDatabase();

            return _database.GetRandomSet(999);
        }

        public ModifierSetConfig GetRandomModifiersSet(ContractDifficulty difficulty)
        {
            _database = _configProvider.GetModifierDatabase();

            return _database.GetRandomSet(difficulty, 999);
        }

        private void ApplyModifierSet(IContract contract, ModifierSetConfig set)
        {
            if (contract == null || set == null)
            {
                return;
            }

            List<IContractModifier> modifiers = _factory.CreateFromSet(set);

            foreach (IContractModifier modifier in modifiers)
            {
                contract.AddModifier(modifier);
            }
        }
    }
}