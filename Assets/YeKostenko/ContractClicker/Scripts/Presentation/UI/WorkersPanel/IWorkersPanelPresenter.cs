namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IWorkersPanelPresenter
    {
        void AttachView(IWorkersPanelView view);
        void DetachView();
    }
}