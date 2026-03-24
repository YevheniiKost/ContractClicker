using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Data.Boosters
{
    [Serializable]
    public struct BoosterRarityWeight
    {
        public BoosterRarity Rarity;

        [Min(0f)]
        public float SpawnWeight;
    }
}

