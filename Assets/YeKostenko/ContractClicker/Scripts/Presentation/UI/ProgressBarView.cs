using UnityEngine;
using UnityEngine.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBarView : View
    {
        [SerializeField]
        private Slider _slider;

        public void SetProgress(float progress)
        {
            _slider.value = progress;
        }

        public void SetMaxProgress(float maxProgress)
        {
            _slider.maxValue = maxProgress;
        }

        public void SetMinProgress(float minProgress)
        {
            _slider.minValue = minProgress;
        }

        protected override void OnAwake()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }

            _slider.interactable = false;
        }

        protected override void OnValidateView()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }
        }
    }
}