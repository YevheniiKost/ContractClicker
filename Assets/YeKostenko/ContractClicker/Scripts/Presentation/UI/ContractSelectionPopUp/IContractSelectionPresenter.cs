namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IContractSelectionPresenter
    {
        void AttachView(IContractSelectionView view);
        void DetachView();
    }
}