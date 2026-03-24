using System;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.CoreKit.UI.Adapters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    internal class BoosterShopSlotAdapter : BaseListAdapter<BoosterShopSlotView, BoosterShopSlotArgs>
    {
        public BoosterShopSlotAdapter(BoosterShopSlotView viewRef, Transform content) : base(viewRef, content)
        {
        }

        public Action<int> SlotClick { get; set; }

        protected override void BindView(int position, BoosterShopSlotView view, BoosterShopSlotArgs data)
        {
            view.SetName(data.Slot.Config.DisplayName);
            view.SetRarityText(FormatRarity(data.Slot.Config.Rarity));
            view.SetPriceText($"{data.Slot.Config.Price}");
            view.SetDurationText(FormatDuration(data.Slot.Config.DurationInSeconds));
            view.SetDescriptionText(data.Slot.Config.Description);
            view.SetActive(!data.Slot.IsPurchased);
            view.SetBackgroundColor(data.Slot.Config.RarityColor);

            view.ButtonClicked = () => SlotClick?.Invoke(data.SlotIndex);
        }

        private static string FormatRarity(BoosterRarity rarity)
        {
            switch (rarity)
            {
                case BoosterRarity.Common:
                    return "Common";
                case BoosterRarity.Rare:
                    return "Rare";
                case BoosterRarity.Epic:
                    return "Epic";
                case BoosterRarity.Legendary:
                    return "Legendary";
                default:
                    return rarity.ToString();
            }
        }

        private static string FormatDuration(float totalSeconds)
        {
            int minutes = (int)totalSeconds / 60;
            int seconds = (int)totalSeconds % 60;
            return string.Format("{0:00}:{1:00} min", minutes, seconds);
        }
    }
}


