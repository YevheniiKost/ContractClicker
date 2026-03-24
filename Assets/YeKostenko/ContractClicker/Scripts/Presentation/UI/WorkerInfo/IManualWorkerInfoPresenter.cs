namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public interface IManualWorkerInfoPresenter
    {
        void AttachView(IManualWorkerInfoView view);
        void DetachView();
    }
}