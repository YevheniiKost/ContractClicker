using System;

using UnityEngine;

using YeKostenko.CoreKit.Extensions;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    internal class WorkerSlotView : View
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
        private TextView _levelText;
        [SerializeField]
        private TextView _workText;

        [SerializeField]
        private ObjectSlotState _currentState;

        public Action SelfButtonClicked { get; set; }

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
            }
        }

        public void SetLevel(int level)
        {
            _levelText.SetText($"Lvl. {level}");
        }

        public void SetWorkText(float yieldPerSecond)
        {
            _workText.SetText($"{yieldPerSecond.ToStatString()}/s");
        }

        protected override void OnAwake()
        {
            _selfButton.ButtonClicked += () => SelfButtonClicked?.Invoke();
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