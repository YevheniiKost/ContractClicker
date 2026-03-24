using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class PlayerProgressPanelView : View, IPlayerProgressPanelView
    {
        [SerializeField]
        private TextView _playerGoldTextView;
        [SerializeField]
        private TextView _manualWorkProgressTextView;
        [SerializeField]
        private TextView _autoWorkProgressTextView;
        [SerializeField]
        private TextView _totalProgressTextView;

        private IPlayerProgressPanelPresenter _presenter;

        public event Action Create;

        public void Initialize(IPlayerProgressPanelPresenter presenter)
        {
            _presenter = presenter;
            _presenter.AttachView(this);

            Create?.Invoke();
        }

        public void SetPlayerGold(int amount)
        {
            _playerGoldTextView.SetText($"Gold: {amount}");
        }

        public void SetWorkProgress(float manualProgress, float autoProgress)
        {
            _manualWorkProgressTextView.SetText($"Manual Work: {manualProgress:F1}");
            _autoWorkProgressTextView.SetText($"Auto Work: {autoProgress:F1}");
            _totalProgressTextView.SetText($"Total Progress: {(manualProgress + autoProgress):F1}");
        }

        protected override void OnDestroyView()
        {
            _presenter.DetachView();
            base.OnDestroyView();
        }
    }
}