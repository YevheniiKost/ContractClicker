using System;

using UnityEngine;

using YeKostenko.ContractClicker.Presentation.UI;

namespace YeKostenko.ContractClicker.Presentation
{
    public class WorkButtonView : View, IWorkButtonView
    {
        [SerializeField]
        private ButtonView _workButton;
        [SerializeField]
        private ButtonView _infoButton;
        [SerializeField]
        private TextView _infoText;

        private IWorkButtonPresenter _presenter;
        private IContractClickerNavigation _navigation;

        public event Action Create;
        public event Action WorkClick;
        public event Action InfoClick;

        public void Initialize(IWorkButtonPresenter presenter,
            IContractClickerNavigation navigation)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            _workButton.ButtonClicked = () => WorkClick?.Invoke();
            _infoButton.ButtonClicked = () => InfoClick?.Invoke();

            _presenter.AttachView(this);

            Create?.Invoke();
        }

        public void SetData(int level, float clickPower)
        {
            _infoText.SetText($"Lvl {level}\nPower {clickPower:F1}/click");
        }

        public void OpenManualWorkerInfoPopUp()
        {
            _navigation.OpenManualWorkerInfoPopUp();
        }

        protected override void OnDestroyView()
        {
            _presenter.DetachView();

            _workButton.ButtonClicked = null;
            _infoButton.ButtonClicked = null;

            base.OnDestroyView();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WorkClick?.Invoke();
            }
        }
#endif
    }
}