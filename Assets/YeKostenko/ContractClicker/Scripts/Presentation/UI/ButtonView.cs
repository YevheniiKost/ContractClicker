using System;

using UnityEngine;
using UnityEngine.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonView : View
    {
        [SerializeField]
        private Button _button;

        public Action ButtonClicked { get; set; }

        public void SetInteractable(bool interactable)
        {
            if (_button != null)
            {
                _button.interactable = interactable;
            }
        }

        protected override void OnAwake()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }

            _button.onClick.AddListener(OnButtonClick);
        }

        protected override void OnValidateView()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnButtonClick()
        {
            ButtonClicked?.Invoke();
        }
    }
}