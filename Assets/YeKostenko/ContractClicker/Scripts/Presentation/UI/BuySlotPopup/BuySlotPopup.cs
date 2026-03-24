using System;

using UnityEngine;

using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuySlotPopup : ContractClickerWindow, IBuySlotView
    {
        [SerializeField]
        private TextView _titleView;
        [SerializeField]
        private TextView _priceView;
        [SerializeField]
        private ButtonView _buyButton;
        [SerializeField]
        private ButtonView _closeButton;

        private IBuySlotPresenter _presenter;
        private BuySlotUIContext _context;

        public event Action<BuySlotPopupType> Create;
        public event Action BuyButtonClick;
        public event Action CloseButtonClick;

        [Inject]
        public void Construct(IBuySlotPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetTitleText(string text) => _titleView.SetText(text);

        public void SetPrice(string text) => _priceView.SetText(text);

        public void SetBuyButtonInteractable(bool interactable) => _buyButton.SetInteractable(interactable);

        public void Close()
        {
            _context.OnClose?.Invoke();
            CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            base.OnCreateWindow(context);

            _context = context as BuySlotUIContext ??
                       throw new ArgumentException("Context must be of type BuySlotUIContext", nameof(context));

            _buyButton.ButtonClicked = () => BuyButtonClick?.Invoke();
            _closeButton.ButtonClicked = () => CloseButtonClick?.Invoke();

            _presenter.AttachView(this);

            Create?.Invoke(_context.PopupType);
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            _buyButton.ButtonClicked = null;
            _closeButton.ButtonClicked = null;

            _presenter.DetachView();
            _presenter = null;
        }
    }
}