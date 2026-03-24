using System;

using UnityEngine;

using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class MessageBoxWindow : ContractClickerWindow, IMessageBoxView
    {
        [SerializeField]
        private TextView _titleText;
        [SerializeField]
        private GameObject _titleObject;

        [SerializeField]
        private TextView _messageText;

        [SerializeField]
        private ButtonView _yesButton;

        [SerializeField]
        private TextView _yesButtonText;

        [SerializeField]
        private ButtonView _noButton;

        [SerializeField]
        private TextView _noButtonText;

        private IMessageBoxPresenter _presenter;

        public event Action<MessageBoxUIContext> Create;
        public event Action YesButtonClick;
        public event Action NoButtonClick;

        [Inject]
        public void Construct(IMessageBoxPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetTitle(string text) => _titleText.SetText(text);

        public void SetTitleVisible(bool visible)
        {
            _titleObject.SetActive(visible);
        }

        public void SetMessage(string text) => _messageText.SetText(text);
        public void SetYesButtonText(string text) => _yesButtonText.SetText(text);
        public void SetNoButtonText(string text) => _noButtonText.SetText(text);

        public void SetNoButtonVisible(bool visible)
        {
            _noButton.gameObject.SetActive(visible);
        }

        public void Close()
        {
            CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            base.OnCreateWindow(context);

            MessageBoxUIContext typedContext = context as MessageBoxUIContext
                ?? throw new ArgumentException($"Context must be {nameof(MessageBoxUIContext)}", nameof(context));

            _yesButton.ButtonClicked = () => YesButtonClick?.Invoke();
            _noButton.ButtonClicked = () => NoButtonClick?.Invoke();

            _presenter.AttachView(this);
            Create?.Invoke(typedContext);
        }

        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            _yesButton.ButtonClicked = null;
            _noButton.ButtonClicked = null;

            _presenter.DetachView();
            _presenter = null;
        }
    }
}


