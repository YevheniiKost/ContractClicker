using System;
using System.Collections.Generic;
using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class WorkersPanelView : View, IWorkersPanelView
    {
        [SerializeField]
        private Transform _slotsContainer;

        [Header("Resources References")]
        [SerializeField]
        private WorkerSlotView _workerSlotPrefab;

        private IWorkersPanelPresenter _presenter;
        private IContractClickerNavigation _navigation;
        private WorkerSlotAdapter _workerSlotAdapter;

        public event Action Create;
        public event Action<int> WorkerSlotClick;

        public void Initialize(IWorkersPanelPresenter presenter, IContractClickerNavigation navigation)
        {
            _presenter = presenter;
            _navigation = navigation;

            _workerSlotAdapter = new WorkerSlotAdapter(_workerSlotPrefab, _slotsContainer);
            _workerSlotAdapter.SlotClick += OnSlotClick;

            _presenter.AttachView(this);

            Create?.Invoke();
        }

        public void SetWorkersData(List<WorkerSlotArgs> slots)
        {
            _workerSlotAdapter.SetData(slots);
        }

        public void OpenBuySlotPopup(Action onClose)
        {
            _navigation.OpenBuyWorkerSlotPopup(onClose);
        }

        public void OpenBuyWorkerPopup(Action onClose)
        {
            _navigation.OpenBuyWorkerPopup(onClose);
        }

        public void OpenWorkerInfoPopup(int id, Action onClose)
        {
            _navigation.OpenWorkerInfoPopup(id, onClose);
        }


        private void OnSlotClick(int slotIndex)
        {
            WorkerSlotClick?.Invoke(slotIndex);
        }
    }
}