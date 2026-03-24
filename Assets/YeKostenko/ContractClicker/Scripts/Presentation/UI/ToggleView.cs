using System;

using UnityEngine;
using UnityEngine.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleView : View
    {
        [SerializeField]
        private Toggle _toggle;

        public bool IsOn
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }

        public Action<bool> IsOnChanged { get; set; }

        protected override void OnAwake()
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }

            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected override void OnValidateView()
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
        }

        protected override void OnDestroyView() => _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        private void OnToggleValueChanged(bool isOn) => IsOnChanged?.Invoke(isOn);
    }
}