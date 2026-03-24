using System;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    [Serializable]
    public abstract class ModifierConfigBase
    {
        public abstract string ModifierType { get; }

        public abstract string GetDescription();
    }
}

