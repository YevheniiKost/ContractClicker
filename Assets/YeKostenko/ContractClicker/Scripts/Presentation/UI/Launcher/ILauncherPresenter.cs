namespace YeKostenko.ContractClicker.Presentation.UI
{
    public interface ILauncherPresenter
    {
        void AttachView(ILauncherView view);
        void DetachView();
    }
}