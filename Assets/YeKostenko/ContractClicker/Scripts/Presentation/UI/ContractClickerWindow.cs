using Cysharp.Threading.Tasks;

using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractClickerWindow : UIWindow
    {
        public override async UniTask OnCloseAsync()
        {
            await base.OnCloseAsync();
            OnDestroyWindow();
        }

        public override async UniTask OnCreateAsync(IUIContext context)
        {
            OnCreateWindow(context);
            await base.OnCreateAsync(context);
            OnWindowReady();
        }

        protected virtual void OnDestroyWindow(){}
        protected virtual void OnCreateWindow(IUIContext context){}
        protected virtual void OnWindowReady() {}

        protected void CloseView()
        {
            UIRoot.Instance.UIManager.CloseTopWindowAsync().Forget();
        }
    }
}