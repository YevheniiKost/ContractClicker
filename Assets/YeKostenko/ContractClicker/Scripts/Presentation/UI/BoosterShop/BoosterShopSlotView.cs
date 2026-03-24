using System;
using UnityEngine;
using UnityEngine.UI;

using YeKostenko.CoreKit.Extensions;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BoosterShopSlotView : ButtonView
    {
        [Header("Content")]
        [SerializeField]
        private TextView _nameText;

        [SerializeField]
        private TextView _rarityText;

        [SerializeField]
        private TextView _priceText;

        [SerializeField]
        private TextView _durationText;

        [SerializeField]
        private TextView _descriptionText;

        [Header("Available State")]
        [SerializeField]
        private GameObject[] _availableStateObjects;

        [Header("Purchased State")]
        [SerializeField]
        private GameObject[] _purchasedStateObjects;

        [Header("Visual")]
        [SerializeField]
        private Image _backgroundImage;

        public void SetActive(bool isActive)
        {
            _availableStateObjects.SetActive(isActive);
            _purchasedStateObjects.SetActive(!isActive);
            SetInteractable(isActive);
        }

        public void SetName(string text) => _nameText.SetText(text);
        public void SetRarityText(string text) => _rarityText.SetText(text);
        public void SetPriceText(string text) => _priceText.SetText(text);
        public void SetDurationText(string text) => _durationText.SetText(text);
        public void SetDescriptionText(string text) => _descriptionText.SetText(text);

        public void SetBackgroundColor(Color color)
        {
            if (_backgroundImage != null)
            {
                _backgroundImage.color = color;
            }
        }
    }
}
