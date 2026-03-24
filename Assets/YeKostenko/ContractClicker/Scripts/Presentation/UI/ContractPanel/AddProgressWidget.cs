using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class AddProgressWidget : View
    {
        [SerializeField]
        private ButtonView _dimView;
        [SerializeField]
        private ButtonView _submitButton;
        [SerializeField]
        private Transform _widgetView;
        [SerializeField]
        private SliderView _progressSlider;

        private Action<float> _submitAction;

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void SetSubmitAction(Action<float> onSubmit)
        {
            _submitAction = onSubmit;
        }

        public void SetMaxProgress(float maxProgress)
        {
            _progressSlider.SetMaxValue(maxProgress);
            _progressSlider.SetMinValue(0);
            _progressSlider.Value = maxProgress / 2;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _submitButton.ButtonClicked = OnSubmitButtonClicked;
            _dimView.ButtonClicked = () => SetVisible(false);
        }

        protected override void OnDestroyView()
        {
            base.OnDestroyView();
            _submitButton.ButtonClicked = null;
        }

        private void OnSubmitButtonClicked()
        {
            _submitAction?.Invoke(_progressSlider.Value);
            SetVisible(false);
        }
    }
}