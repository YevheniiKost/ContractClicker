using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.ContractClicker.Domain.Economy;

using YevheniiKostenko.CoreKit.Time;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterShop : IBoosterShop, ITimeListener
    {
        private const float DefaultRarityWeight = 1f;

        private readonly BoosterShopConfig _config;
        private readonly ITimeProvider _timeProvider;
        private readonly List<BoosterShopSlot> _currentSlots;

        private float _timeUntilNextRefresh;

        public BoosterShop(BoosterShopConfig config, ITimeProvider timeProvider)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

            _currentSlots = new List<BoosterShopSlot>(_config.SlotsCount);
            _timeUntilNextRefresh = _config.ShopRefreshIntervalInSeconds;

            _timeProvider.RegisterTimeListener(this);
            RefreshSlots();
        }

        public event EventHandler ShopRefreshed;
        public event EventHandler<BoosterPurchasedEventArgs> BoosterPurchased;

        public IReadOnlyList<BoosterShopSlot> CurrentSlots => _currentSlots;
        public float TimeUntilNextRefreshInSeconds => _timeUntilNextRefresh;

        public bool TryPurchaseBooster(int slotIndex, IPlayerWallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }

            if (!IsValidSlotIndex(slotIndex))
            {
                return false;
            }

            BoosterShopSlot slot = _currentSlots[slotIndex];

            if (slot.IsPurchased)
            {
                return false;
            }

            if (!wallet.TrySpendFunds(CurrencyType.Gold, slot.Config.Price))
            {
                return false;
            }

            slot.MarkAsPurchased();

            BoosterPurchased?.Invoke(this, new BoosterPurchasedEventArgs(slot, slotIndex));
            return true;
        }

        public void ForceRefresh()
        {
            RefreshSlots();
        }

        public void Update(float deltaTime)
        {
            _timeUntilNextRefresh -= deltaTime;

            if (_timeUntilNextRefresh <= 0f)
            {
                RefreshSlots();
            }
        }

        public void Dispose()
        {
            _timeProvider.ClearTimeListener(this);
        }

        private bool IsValidSlotIndex(int slotIndex)
        {
            return slotIndex >= 0 && slotIndex < _currentSlots.Count;
        }

        private void RefreshSlots()
        {
            _currentSlots.Clear();
            _timeUntilNextRefresh = _config.ShopRefreshIntervalInSeconds;

            List<BoosterConfig> pool = new List<BoosterConfig>(_config.AvailableBoosters);
            int slotsToFill = Mathf.Min(_config.SlotsCount, pool.Count);

            for (int i = 0; i < slotsToFill; i++)
            {
                int selectedIndex = PickWeightedRandomIndex(pool);
                _currentSlots.Add(new BoosterShopSlot(pool[selectedIndex]));
                pool.RemoveAt(selectedIndex);
            }

            SortSlotsByRarityDescending();
            ShopRefreshed?.Invoke(this, EventArgs.Empty);
        }

        private int PickWeightedRandomIndex(List<BoosterConfig> pool)
        {
            float totalWeight = 0f;
            for (int i = 0; i < pool.Count; i++)
            {
                totalWeight += GetRarityWeight(pool[i].Rarity);
            }

            if (totalWeight <= 0f)
            {
                return UnityEngine.Random.Range(0, pool.Count);
            }

            float roll = UnityEngine.Random.Range(0f, totalWeight);
            float accumulated = 0f;

            for (int i = 0; i < pool.Count; i++)
            {
                accumulated += GetRarityWeight(pool[i].Rarity);
                if (roll <= accumulated)
                {
                    return i;
                }
            }

            return pool.Count - 1;
        }

        private float GetRarityWeight(BoosterRarity rarity)
        {
            IReadOnlyList<BoosterRarityWeight> weights = _config.RarityWeights;

            if (weights == null)
            {
                return DefaultRarityWeight;
            }

            for (int i = 0; i < weights.Count; i++)
            {
                if (weights[i].Rarity == rarity)
                {
                    return weights[i].SpawnWeight;
                }
            }

            return DefaultRarityWeight;
        }

        private void SortSlotsByRarityDescending()
        {
            for (int i = 1; i < _currentSlots.Count; i++)
            {
                BoosterShopSlot key = _currentSlots[i];
                int j = i - 1;

                while (j >= 0 && (int)_currentSlots[j].Config.Rarity < (int)key.Config.Rarity)
                {
                    _currentSlots[j + 1] = _currentSlots[j];
                    j--;
                }

                _currentSlots[j + 1] = key;
            }
        }
    }
}

