using System.Collections.Generic;
using System.Linq;

using YeKostenko.ContractClicker.Domain.Boosters;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class WorkersPanelPresenter : IWorkersPanelPresenter
    {
        private readonly IWorkController _workController;
        private readonly IBoosterManager  _boosterManager;

        private readonly Dictionary<int, WorkerSlotArgs> _workersSlots = new();

        private IWorkersPanelView _view;

        public WorkersPanelPresenter(IWorkController workController, IBoosterManager boosterManager)
        {
            _workController = workController;
            _boosterManager = boosterManager;
        }

        public void AttachView(IWorkersPanelView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.WorkerSlotClick += OnWorkerSlotClick;

            _boosterManager.BoosterActivated += OnBoosterActivated;
            _boosterManager.BoosterExpired += OnBoosterExpired;
        }

        public void DetachView()
        {
            _view.Create -= OnCreate;
            _view.WorkerSlotClick -= OnWorkerSlotClick;
            _view = null;

            _boosterManager.BoosterActivated -= OnBoosterActivated;
            _boosterManager.BoosterExpired -= OnBoosterExpired;
        }

        private void OnCreate()
        {
            UpdateWorkersData();
        }

        private void UpdateWorkersData()
        {
            List<IAutomaticWorker> workers = _workController.AutomaticWorkers;
            int maxSlots = _workController.MaxWorkerSlots;
            int availableSlots = _workController.UnlockedWorkerSlots;

            for (int i = 0; i < maxSlots; i++)
            {
                int index = i + 1;
                if (i < workers.Count)
                {
                    IAutomaticWorker worker = workers[i];
                    _workersSlots[index] = WorkerSlotArgs.WithWorker(index, worker);
                }
                else
                {
                    _workersSlots[index] = WorkerSlotArgs.Empty(index, i >= availableSlots);
                }
            }

            _view.SetWorkersData(_workersSlots.Values.ToList());
        }

        private void OnWorkerSlotClick(int slotIndex)
        {
            if (_workersSlots.TryGetValue(slotIndex, out WorkerSlotArgs slotArgs))
            {
                if (slotArgs.IsEmpty)
                {
                    if (slotArgs.IsLocked)
                    {
                        _view.OpenBuySlotPopup(UpdateWorkersData);
                    }
                    else
                    {
                        _view.OpenBuyWorkerPopup(UpdateWorkersData);
                    }
                }
                else
                {
                    _view.OpenWorkerInfoPopup(slotArgs.Worker.Id, UpdateWorkersData);
                }
            }
        }

        private void OnBoosterActivated(object sender, BoosterActivatedEventArgs args) => UpdateWorkersData();

        private void OnBoosterExpired(object sender, BoosterExpiredEventArgs e) => UpdateWorkersData();
    }
}