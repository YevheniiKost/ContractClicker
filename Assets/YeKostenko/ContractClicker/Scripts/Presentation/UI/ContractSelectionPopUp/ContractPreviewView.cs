using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractPreviewView : View
    {
        [SerializeField]
        private Button _selfButton;

        [Space]
        [SerializeField]
        private TextView _nameText;
        [SerializeField]
        private TextView _rewardText;
        [SerializeField]
        private TextView _progressText;

        [Header("Modifiers")]
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Transform _modifiersContainer;

        [SerializeField]
        private ContractModifierPreviewView _modifierPreviewPrefab;

        private ContractModifierPreviewAdapter _modifierPreviewAdapter;

        public Action OnClick { get; set; }

        public void SetName(string name)
        {
            _nameText.SetText(name);
        }

        public void SetReward(int reward)
        {
            _rewardText.SetText($"{reward}");
        }

        public void SetProgress(float required)
        {
            _progressText.SetText($"Need work: {required:F1}");
        }

        public void SetBackgroundColor(Color color)
        {
            _backgroundImage.color = color;
        }

        public void SetModifiers(List<string> modifiers)
        {
            _modifierPreviewAdapter.SetData(modifiers);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_modifiersContainer.transform as RectTransform);
        }

        protected override void OnAwake()
        {
            _selfButton.onClick.AddListener(() => OnClick?.Invoke());

            _modifierPreviewAdapter = new ContractModifierPreviewAdapter(_modifierPreviewPrefab, _modifiersContainer);
        }

        protected override void OnDestroyView()
        {
            _selfButton.onClick.RemoveAllListeners();
        }
    }
}