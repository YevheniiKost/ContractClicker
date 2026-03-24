using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IMessageBoxView
    {
        event Action<MessageBoxUIContext> Create;
        event Action YesButtonClick;
        event Action NoButtonClick;

        void SetTitle(string text);
        void SetTitleVisible(bool visible);
        void SetMessage(string text);
        void SetYesButtonText(string text);
        void SetNoButtonText(string text);
        void SetNoButtonVisible(bool visible);
        void Close();
    }
}


