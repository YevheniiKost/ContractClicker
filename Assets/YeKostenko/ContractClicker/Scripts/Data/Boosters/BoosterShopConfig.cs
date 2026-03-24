using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Object = UnityEngine.Object;

namespace YeKostenko.ContractClicker.Data.Boosters
{
    [CreateAssetMenu(fileName = "BoosterShopConfig", menuName = "ContractClicker/Boosters/Booster Shop Config")]
    public class BoosterShopConfig : ScriptableObject
    {
        [Header("Refresh")]
        [SerializeField]
        [Min(1f)]
        private float _shopRefreshIntervalInSeconds = 300f;

        [Header("Slots")]
        [SerializeField]
        [Min(1)]
        private int _slotsCount = 3;

        [Header("Available Boosters Pool")]
        [SerializeField]
        private List<BoosterConfig> _availableBoosters;

        [Header("Rarity Spawn Weights")]
        [SerializeField]
        private BoosterRarityWeight[] _rarityWeights;

        public static async UniTask<BoosterShopConfig> Load()
        {
            Object asset = await Resources.LoadAsync<BoosterShopConfig>("BoosterShopConfig");
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            return asset as BoosterShopConfig;
        }

        public float ShopRefreshIntervalInSeconds => _shopRefreshIntervalInSeconds;
        public int SlotsCount => _slotsCount;
        public IReadOnlyList<BoosterConfig> AvailableBoosters => _availableBoosters;
        public IReadOnlyList<BoosterRarityWeight> RarityWeights => _rarityWeights;
    }
}

