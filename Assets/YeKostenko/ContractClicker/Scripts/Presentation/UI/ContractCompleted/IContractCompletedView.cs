using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractCompletedView
    {
        event Action<int> Create;
        event Action CloseButtonClick;

        void SetTitle(string title);
        void SetRewardText(string rewardText);
        void Close();
    }
}