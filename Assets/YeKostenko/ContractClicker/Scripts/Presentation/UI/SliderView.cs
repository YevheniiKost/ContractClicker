using System;

using UnityEngine;
using UnityEngine.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    public class SliderView : View
    {
        [SerializeField]
        private Slider _slider;

        [Header("Additional Settings")]
        [SerializeField]
        private TextView _minValueText;
        [SerializeField]
        private TextView _maxValueText;
        [SerializeField]
        private TextView _progressValueText;

        [Space]
        [SerializeField]
        private bool _wholeNumbersOnly;

        public Action<float> ValueChanged { get; set; }

        public float Value
        {
            get => _slider.value;
            set => _slider.value = value;
        }

        public void SetMaxValue(float maxValue)
        {
            _slider.maxValue = maxValue;
            if (_maxValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _maxValueText.SetText(string.Format(textFormat, maxValue));
            }
        }

        public void SetMinValue(float minValue)
        {
            _slider.minValue = minValue;
            if(_minValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _minValueText.SetText(string.Format(textFormat, minValue));
            }
        }

        protected override void OnAwake()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }

            _slider.wholeNumbers = _wholeNumbersOnly;
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected override void OnValidateView()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }

            if (_minValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _minValueText.SetText(string.Format(textFormat, _slider.minValue));
            }

            if (_maxValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _maxValueText.SetText(string.Format(textFormat, _slider.maxValue));
            }

            if (_progressValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _progressValueText.SetText(string.Format(textFormat, _slider.value));
            }
        }

        protected override void OnDestroyView()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            base.OnDestroyView();
        }

        private void OnSliderValueChanged(float value)
        {
            ValueChanged?.Invoke(value);
            if (_progressValueText != null)
            {
                string textFormat = GetNumbersFormat();
                _progressValueText.SetText(string.Format(textFormat, value));
            }
        }

        private string GetNumbersFormat() => _wholeNumbersOnly ? "{0:0}" : "{0:0.#}";
    }
}