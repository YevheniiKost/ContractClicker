namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractPanelPresenter
    {
        void AttachView(IContractPanelView view);
        void DetachView();
    }
}