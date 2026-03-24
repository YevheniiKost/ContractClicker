using System;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Data.Boosters
{
    [Serializable]
    public struct BoosterStatModificationData
    {
        public BoosterTarget Target;
        public StatId Stat;
        public float Value;
        public ModificationOperation Operation;
    }
}

