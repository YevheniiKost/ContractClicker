using System;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class MessageBoxUIContext : IUIContext
    {
        public string Message { get; }
        public string Title { get; }
        public string YesButtonText { get; }
        public string NoButtonText { get; }
        public Action OnYes { get; }
        public Action OnNo { get; }

        public MessageBoxUIContext(
            string message,
            string title = null,
            string yesButtonText = null,
            string noButtonText = null,
            Action onYes = null,
            Action onNo = null)
        {
            Message = message;
            Title = title;
            YesButtonText = yesButtonText;
            NoButtonText = noButtonText;
            OnYes = onYes;
            OnNo = onNo;
        }
    }
}
