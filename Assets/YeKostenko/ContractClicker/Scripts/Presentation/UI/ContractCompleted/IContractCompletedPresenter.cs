namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractCompletedPresenter
    {
        void AttachView(IContractCompletedView view);
        void DetachView();
    }
}