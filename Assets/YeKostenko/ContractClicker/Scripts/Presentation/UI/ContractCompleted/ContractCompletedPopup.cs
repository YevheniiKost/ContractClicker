using System;

using UnityEngine;

using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractCompletedPopup : ContractClickerWindow , IContractCompletedView
    {
        [SerializeField]
        private TextView _titleTextView;
        [SerializeField]
        private TextView _rewardTextView;
        [SerializeField]
        private ButtonView _closeButton;

        private IContractCompletedPresenter _presenter;
        private ContractCompletedUIContext _context;

        public event Action<int> Create;
        public event Action CloseButtonClick;

        [Inject]
        public void Construct(IContractCompletedPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetTitle(string title)
        {
             _titleTextView.SetText(title);
        }

        public void SetRewardText(string rewardText)
        {
            _rewardTextView.SetText(rewardText);
        }

        public void Close()
        {
            _context?.CloseCallback?.Invoke();
            CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            _context = context as ContractCompletedUIContext ??
                       throw new ArgumentException("Invalid context type for ContractCompletedPopup");

            _closeButton.ButtonClicked += () => CloseButtonClick?.Invoke();

            _presenter.AttachView(this);

            Create?.Invoke(_context.ContractId);
        }

        protected override void OnDestroyWindow()
        {
            _presenter.DetachView();
            _closeButton.ButtonClicked = null;
            _presenter = null;
        }
    }
}