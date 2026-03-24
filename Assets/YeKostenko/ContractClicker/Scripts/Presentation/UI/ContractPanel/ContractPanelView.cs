using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractPanelView : View, IContractPanelView
    {
        [SerializeField]
        private Transform _contractSlotsContainer;
        [SerializeField]
        private AddProgressWidget _addProgressWidget;

        [Header("Resources References")]
        [SerializeField]
        private ContractSlotView _contractSlotViewPrefab;

        private IContractClickerNavigation _navigation;
        private IContractPanelPresenter _presenter;
        private ContractSlotsAdapter _contractSlotsAdapter;

        public event Action Create;
        public event Action<int> SlotClicked;
        public event Action<int> FillContractClicked;
        public event Action<int, bool> AutoFillToggled;

        public void Initialize(IContractClickerNavigation navigation,
            IContractPanelPresenter presenter)
        {
            _navigation = navigation;
            _presenter = presenter;

            Bind();

            Create?.Invoke();
        }

        protected override void OnDestroyView()
        {
            Unbind();
            base.OnDestroyView();
        }

        public void SetContractsData(List<ContractSlotArgs> contracts)
        {
            _contractSlotsAdapter.SetData(contracts);
        }

        public void OpenContractSelectionPopUp(Action<ContractDefinition> onSelect)
        {
            _navigation.OpenContractSelectionPopUp(onSelect);
        }

        public void OpenContractCompletePopUp(int contractId, Action onClose)
        {
            _navigation.OpenContractCompletePopUp(contractId, onClose);
        }

        public void OpenContractInfoPopUp(int contractId, Action onClose)
        {
            _navigation.OpenContractInfoPopUp(contractId, onClose);
        }

        public void OpenBuyContractSlotPopup(Action onClose) => _navigation.OpenBuyContractSlotPopup(onClose);

        public void OpenFillContractWidget(int slotId, float progressToShow, Action<float> addedProgress)
        {
            _addProgressWidget.SetMaxProgress(progressToShow);
            _addProgressWidget.SetSubmitAction((value) =>
            {
                addedProgress?.Invoke(value);
                _addProgressWidget.SetVisible(false);
            });
            _addProgressWidget.SetVisible(true);
        }

        private void Bind()
        {
            if (_contractSlotsAdapter != null)
            {
                _contractSlotsAdapter.ClearData();
            }

            _contractSlotsAdapter = new ContractSlotsAdapter(_contractSlotViewPrefab, _contractSlotsContainer)
            {
                ContractClicked = (contractId) => SlotClicked?.Invoke(contractId),
                FillButtonClicked = (contractId) => FillContractClicked?.Invoke(contractId),
                AutoFillToggleChanged = (contractId, isOn) => AutoFillToggled?.Invoke(contractId, isOn)
            };

            _presenter.AttachView(this);
        }

        private void Unbind()
        {
            _presenter.DetachView();

            _contractSlotsAdapter.ClearData();
            _contractSlotsAdapter.ContractClicked = null;
            _contractSlotsAdapter.FillButtonClicked = null;
            _contractSlotsAdapter.AutoFillToggleChanged = null;
            _contractSlotsAdapter = null;
            _presenter = null;
            _navigation = null;
        }
    }
}