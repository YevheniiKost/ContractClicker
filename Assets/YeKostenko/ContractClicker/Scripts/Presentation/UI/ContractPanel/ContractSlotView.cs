using System;

using UnityEngine;

using YeKostenko.CoreKit.Extensions;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    internal class ContractSlotView : View
    {
        [SerializeField]
        private ButtonView _selfButton;

        [Header("Empty State")]
        [SerializeField]
        private GameObject[] _emptyStateObjects;

        [Header("Locked State")]
        [SerializeField]
        private GameObject[] _lockedStateObjects;

        [Header("Active State")]
        [SerializeField]
        private GameObject[] _activeStateObjects;
        [SerializeField]
        private TextView _nameText;
        [SerializeField]
        private TextView _progressText;
        [SerializeField]
        private ProgressBarView _progressBar;
        [SerializeField]
        private ButtonView _fillButton;
        [SerializeField]
        private ToggleView _autoFillToggle;

        [Header("Completed State")]
        [SerializeField]
        private GameObject[] _completedStateObjects;

        [Header("States")]
        [SerializeField]
        private ObjectSlotState _currentState;

        public Action SelfButtonClicked { get; set; }
        public Action FillButtonClicked { get; set; }
        public Action<bool> AutoFillToggleChanged { get; set; }

        public Vector3 FillButtonPosition => _fillButton.transform.position;

        public void SetState(ObjectSlotState state, bool force = false)
        {
            if (!force && _currentState == state)
            {
                return;
            }

            _currentState = state;
            _emptyStateObjects.SetActive(false);
            _lockedStateObjects.SetActive(false);
            _activeStateObjects.SetActive(false);
            _completedStateObjects.SetActive(false);

            switch (state)
            {
                case ObjectSlotState.Empty:
                    _emptyStateObjects.SetActive(true);
                    break;
                case ObjectSlotState.Locked:
                    _lockedStateObjects.SetActive(true);
                    break;
                case ObjectSlotState.Active:
                    _activeStateObjects.SetActive(true);
                    break;
                case ObjectSlotState.Completed:
                    _completedStateObjects.SetActive(true);
                    break;
            }
        }

        public void SetContractName(string contactName) => _nameText.SetText(contactName);

        public void SetProgress(float currentProgress, float maxProgress)
        {
            _progressText.SetText($"{currentProgress:F1}/{maxProgress:F1}");
            _progressBar.SetMaxProgress(maxProgress);
            _progressBar.SetProgress(currentProgress);
        }

        public void SetAutoFillToggle(bool isOn)
        {
            _autoFillToggle.IsOn = isOn;
        }

        protected override void OnAwake()
        {
            _selfButton.ButtonClicked += () => SelfButtonClicked?.Invoke();
            _fillButton.ButtonClicked += () => FillButtonClicked?.Invoke();
            _autoFillToggle.IsOn = false;
            _autoFillToggle.IsOnChanged += isOn => AutoFillToggleChanged?.Invoke(isOn);
        }

        protected override void OnDestroyView()
        {
            _selfButton.ButtonClicked = null;
            _fillButton.ButtonClicked = null;
            _autoFillToggle.IsOnChanged = null;
        }

        protected override void OnValidateView()
        {
            try
            {
                SetState(_currentState, true);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}