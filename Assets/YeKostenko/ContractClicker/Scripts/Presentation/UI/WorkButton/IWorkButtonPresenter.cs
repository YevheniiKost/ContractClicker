namespace YeKostenko.ContractClicker.Presentation
{
    public interface IWorkButtonPresenter
    {
        void AttachView(IWorkButtonView view);
        void DetachView();
    }
}