using System;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface ILauncherView
    {
        event Action Create;
        void OpenMainMenu();

        void SetLoadingProgress(float progress);
    }
}