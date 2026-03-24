using System;

using UnityEngine;

using YeKostenko.CoreKit.DI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class LauncherWindow : ContractClickerWindow, ILauncherView
    {
        [SerializeField]
        private ProgressBarView _progressBar;
        [SerializeField]
        private TextView _progressBarText;

        private IContractClickerNavigation _navigation;
        private ILauncherPresenter _presenter;

        public event Action Create;

        [Inject]
        public void Construct(IContractClickerNavigation navigation,
            ILauncherPresenter presenter)
        {
            _navigation = navigation;
            _presenter = presenter;

            _presenter.AttachView(this);

            Create?.Invoke();
        }

        protected override void OnDestroyWindow()
        {
            _presenter.DetachView();

            base.OnDestroyWindow();
        }


        public void OpenMainMenu()
        {
            _navigation.OpenMainMenu();
        }

        public void SetLoadingProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);

            _progressBar.SetProgress(progress);
            _progressBarText.SetText($"{Mathf.RoundToInt(progress * 100f)}%");
        }
    }
}