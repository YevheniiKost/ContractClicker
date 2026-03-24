using System;

namespace YeKostenko.ContractClicker.Presentation
{
    public interface IWorkButtonView
    {
        event Action Create;
        event Action WorkClick;
        event Action InfoClick;
        void SetData(int level, float clickPower);
        void OpenManualWorkerInfoPopUp();
    }
}