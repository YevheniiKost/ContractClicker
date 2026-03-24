namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IPlayerProgressPanelPresenter
    {
        void AttachView(IPlayerProgressPanelView view);
        void DetachView();
    }
}