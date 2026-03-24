using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IPlayerProgressPanelView
    {
        event Action Create;

        void SetPlayerGold(int amount);
        void SetWorkProgress(float manualProgress, float autoProgress);
    }
}