using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.ContractClicker.Domain.Boosters;
using YeKostenko.ContractClicker.Domain.Economy;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BoosterShopPresenter : IBoosterShopPresenter
    {
        private readonly IBoosterShop _boosterShop;
        private readonly IPlayerWallet _playerWallet;
        private readonly IApplyBoosterUseCase _applyBoosterUseCase;

        private IBoosterShopView _view;

        public BoosterShopPresenter(
            IBoosterShop boosterShop,
            IPlayerWallet playerWallet,
            IApplyBoosterUseCase applyBoosterUseCase)
        {
            _boosterShop = boosterShop ?? throw new ArgumentNullException(nameof(boosterShop));
            _playerWallet = playerWallet ?? throw new ArgumentNullException(nameof(playerWallet));
            _applyBoosterUseCase = applyBoosterUseCase ?? throw new ArgumentNullException(nameof(applyBoosterUseCase));
        }

        public void AttachView(IBoosterShopView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.SlotClick += OnSlotClick;
            _boosterShop.ShopRefreshed += OnBoosterShopShopRefreshed;
            _boosterShop.BoosterPurchased += OnBoosterShopBoosterPurchased;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnCreate;
                _view.SlotClick -= OnSlotClick;
            }

            _boosterShop.ShopRefreshed -= OnBoosterShopShopRefreshed;
            _boosterShop.BoosterPurchased -= OnBoosterShopBoosterPurchased;

            _view = null;
        }

        private void OnCreate()
        {
            RefreshView();
            _view.StartRefreshTimer(_boosterShop.TimeUntilNextRefreshInSeconds);
        }

        private void OnSlotClick(int slotIndex)
        {
            IReadOnlyList<BoosterShopSlot> slots = _boosterShop.CurrentSlots;

            if (!IsValidSlotIndex(slotIndex, slots.Count))
            {
                return;
            }

            BoosterShopSlot slot = slots[slotIndex];

            if (slot.IsPurchased)
            {
                return;
            }

            int balance = _playerWallet.GetBalance(CurrencyType.Gold);

            if (balance < slot.Config.Price)
            {
                _view.ShowInsufficientFundsMessage();
                return;
            }

            _view.ShowPurchaseConfirmation(
                slot.Config.DisplayName,
                slot.Config.Price,
                () => PurchaseBooster(slotIndex));
        }

        private void PurchaseBooster(int slotIndex)
        {
            IReadOnlyList<BoosterShopSlot> slots = _boosterShop.CurrentSlots;

            if (!IsValidSlotIndex(slotIndex, slots.Count))
            {
                return;
            }

            BoosterConfig config = slots[slotIndex].Config;
            bool purchased = _boosterShop.TryPurchaseBooster(slotIndex, _playerWallet);

            if (purchased)
            {
                _applyBoosterUseCase.Execute(config);
            }
        }

        private void OnBoosterShopShopRefreshed(object sender, EventArgs eventArgs)
        {
            RefreshView();
            _view.StartRefreshTimer(_boosterShop.TimeUntilNextRefreshInSeconds);
        }

        private void OnBoosterShopBoosterPurchased(object sender, BoosterPurchasedEventArgs eventArgs)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            IReadOnlyList<BoosterShopSlot> slots = _boosterShop.CurrentSlots;
            List<BoosterShopSlotArgs> args = new List<BoosterShopSlotArgs>(slots.Count);

            for (int i = 0; i < slots.Count; i++)
            {
                args.Add(new BoosterShopSlotArgs(slots[i], i));
            }

            _view.SetSlotsData(args);
        }

        private static bool IsValidSlotIndex(int slotIndex, int slotCount)
        {
            return slotIndex >= 0 && slotIndex < slotCount;
        }
    }
}
