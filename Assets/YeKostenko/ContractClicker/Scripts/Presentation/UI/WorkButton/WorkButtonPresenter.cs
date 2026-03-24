using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Boosters;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation
{
    public class WorkButtonPresenter : IWorkButtonPresenter
    {
        private readonly IWorkController _workController;
        private readonly IBoosterManager  _boosterManager;

        private IManualWorker _manualWorker;
        private IWorkButtonView _view;

        public WorkButtonPresenter(IWorkController workController, IBoosterManager boosterManager)
        {
            _workController = workController;
            _boosterManager = boosterManager;
        }

        public void AttachView(IWorkButtonView view)
        {
            _view = view;

            _view.Create += OnCreate;
            _view.WorkClick += OnWorkClick;
            _view.InfoClick += OnInfoClick;

            _manualWorker = _workController.ManualWorker;
            _manualWorker.LevelUp += UpdateWorkerData;

            _boosterManager.BoosterActivated += OnBoosterActivated;
            _boosterManager.BoosterExpired += OnBoosterExpired;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.WorkClick -= OnWorkClick;
                _view = null;
            }

            if (_manualWorker != null)
            {
                _manualWorker.LevelUp -= UpdateWorkerData;
                _manualWorker = null;
            }

            _boosterManager.BoosterActivated -= OnBoosterActivated;
            _boosterManager.BoosterExpired -= OnBoosterExpired;
        }
        private void OnWorkClick() => _workController.ManualWorker.ProcessClick();

        private void OnCreate() => UpdateWorkerData();

        private void UpdateWorkerData() => _view.SetData(_manualWorker.Level,
            _manualWorker.GetStat(StatId.ClickPower, withLevel: true, withEffects: true));

        private void OnInfoClick() => _view.OpenManualWorkerInfoPopUp();

        private void OnBoosterActivated(object sender, BoosterActivatedEventArgs e) => UpdateWorkerData();

        private void OnBoosterExpired(object sender, BoosterExpiredEventArgs e) => UpdateWorkerData();

    }
}