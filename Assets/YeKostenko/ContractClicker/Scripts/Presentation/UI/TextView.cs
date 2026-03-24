using TMPro;

using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextView : View
    {
        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        protected override void OnAwake()
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<TextMeshProUGUI>();
            }
        }

        protected override void OnValidateView()
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<TextMeshProUGUI>();
            }
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }
    }
}