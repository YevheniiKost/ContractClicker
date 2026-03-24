using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BoosterShopWindow : ContractClickerWindow, IBoosterShopView
    {
        [Header("Components")]
        [SerializeField]
        private Transform _slotsContainer;

        [SerializeField]
        private TimerView _timerView;

        [SerializeField]
        private ButtonView _closeButton;

        [Header("Resources References")]
        [SerializeField]
        private BoosterShopSlotView _slotPrefab;

        private IBoosterShopPresenter _presenter;
        private IContractClickerNavigation _navigation;
        private BoosterShopSlotAdapter _adapter;
        private BoosterShopUIContext _context;

        public event Action Create;
        public event Action<int> SlotClick;

        [Inject]
        public void Construct(IBoosterShopPresenter presenter, IContractClickerNavigation navigation)
        {
            _presenter = presenter;
            _navigation = navigation;
        }

        public void SetSlotsData(List<BoosterShopSlotArgs> slots)
        {
            _adapter.SetData(slots);
        }

        public void StartRefreshTimer(float totalSeconds)
        {
            _timerView.StartTimer(totalSeconds);
        }

        public void ShowInsufficientFundsMessage()
        {
            _navigation.OpenMessageBox(
                "You don't have enough gold to purchase this booster.",
                "Not Enough Gold",
                "OK");
        }

        public void ShowPurchaseConfirmation(string boosterName, int price, Action onConfirm)
        {
            _navigation.OpenMessageBox(
                $"Buy \"{boosterName}\" for {price} G?",
                "Confirm Purchase",
                noButtonText: "No",
                onYes: onConfirm);
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            base.OnCreateWindow(context);

            _context = context as BoosterShopUIContext;

            _adapter = new BoosterShopSlotAdapter(_slotPrefab, _slotsContainer);
            _adapter.SlotClick += OnAdapterSlotClick;

            _closeButton.ButtonClicked = OnCloseButtonClick;

            _presenter.AttachView(this);
            Create?.Invoke();
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            _adapter.SlotClick -= OnAdapterSlotClick;
            _closeButton.ButtonClicked = null;

            _presenter.DetachView();
            _presenter = null;
        }

        private void OnAdapterSlotClick(int slotIndex)
        {
            SlotClick?.Invoke(slotIndex);
        }

        private void OnCloseButtonClick()
        {
            _context?.OnClose?.Invoke();
            CloseView();
        }
    }
}


