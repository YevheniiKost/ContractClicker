using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class MessageBoxPresenter : IMessageBoxPresenter
    {
        private const string DefaultYesText = "Yes";
        private const string DefaultNoText = "No";

        private IMessageBoxView _view;
        private Action _onYes;
        private Action _onNo;

        public void AttachView(IMessageBoxView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.YesButtonClick += OnYesButtonClick;
            _view.NoButtonClick += OnNoButtonClick;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnCreate;
                _view.YesButtonClick -= OnYesButtonClick;
                _view.NoButtonClick -= OnNoButtonClick;
            }

            _view = null;
            _onYes = null;
            _onNo = null;
        }

        private void OnCreate(MessageBoxUIContext context)
        {
            _onYes = context.OnYes;
            _onNo = context.OnNo;

            bool hasTitle = !string.IsNullOrEmpty(context.Title);
            _view.SetTitleVisible(hasTitle);
            if (hasTitle)
            {
                _view.SetTitle(context.Title);
            }

            _view.SetMessage(context.Message);

            string yesText = string.IsNullOrEmpty(context.YesButtonText) ? DefaultYesText : context.YesButtonText;
            _view.SetYesButtonText(yesText);

            bool hasNoButton = context.NoButtonText != null;
            _view.SetNoButtonVisible(hasNoButton);
            if (hasNoButton)
            {
                string noText = string.IsNullOrEmpty(context.NoButtonText) ? DefaultNoText : context.NoButtonText;
                _view.SetNoButtonText(noText);
            }
        }

        private void OnYesButtonClick()
        {
            _onYes?.Invoke();
            _view.Close();
        }

        private void OnNoButtonClick()
        {
            _onNo?.Invoke();
            _view.Close();
        }
    }
}
