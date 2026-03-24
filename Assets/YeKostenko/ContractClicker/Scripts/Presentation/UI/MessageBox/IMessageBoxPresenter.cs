namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface IMessageBoxPresenter
    {
        void AttachView(IMessageBoxView view);
        void DetachView();
    }
}

