using System;
using System.Text;

using UnityEngine;

using YeKostenko.ContractClicker.Domain.Work;
using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuyWorkerPopUp : ContractClickerWindow, IBuyWorkerView
    {
        [SerializeField]
        private TextView _workerStatsTextView;
        [SerializeField]
        private TextView _priceTextView;
        [SerializeField]
        private ButtonView _buyButtonView;
        [SerializeField]
        private ButtonView _closeButtonView;

        private IBuyWorkerPresenter _presenter;
        private BuyWorkerPopUpUIContext _context;

        public event Action Create;
        public event Action BuyButtonClick;
        public event Action CloseButtonClick;

        [Inject]
        public void Construct(IBuyWorkerPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetWorkerData(AutomaticWorkerStats stats)
        {
            StringBuilder statsBuilder = new StringBuilder();
            statsBuilder.AppendLine($"Production: {stats.YieldPerTick} per {stats.TickInterval} seconds");
            statsBuilder.AppendLine($"Critical Chance: {stats.CriticalChance * 100}%");
            string text = statsBuilder.ToString().TrimEnd();

            _workerStatsTextView.SetText(text);
        }

        public void SetPrice(int price) => _priceTextView.SetText(price.ToString());

        public void SetBuyButtonInteractable(bool interactable) => _buyButtonView.SetInteractable(interactable);

        public void Close()
        {
            _context.OnClose?.Invoke();
            CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            _context = context as BuyWorkerPopUpUIContext ??
                       throw new ArgumentException("Invalid context type for BuyWorkerPopUp", nameof(context));

            _buyButtonView.ButtonClicked = () => BuyButtonClick?.Invoke();
            _closeButtonView.ButtonClicked = () => CloseButtonClick?.Invoke();

            _presenter.AttachView(this);

            Create?.Invoke();
        }

        protected override void OnDestroyWindow()
        {
            _presenter.DetachView();
            _buyButtonView.ButtonClicked = null;
            _closeButtonView.ButtonClicked = null;
            _presenter = null;
            _context = null;
        }
    }


}