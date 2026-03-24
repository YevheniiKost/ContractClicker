using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;

using YevheniiKostenko.CoreKit.Time;

namespace YeKostenko.ContractClicker.Domain.Work
{
    public class WorkController : IWorkController, ITimeListener
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IGameConfigProvider _configProvider;

        private SlotsConfig _slotsConfig;

        private bool _isInitialized = false;
        private int _unlockedWorkerSlots;

        public WorkController(ITimeProvider timeProvider, IGameConfigProvider configProvider)
        {
            _timeProvider = timeProvider;
            _configProvider = configProvider;
        }

        public event Action AutomaticWorkersChanged;
        public WorkProgress ManualProgress { get; private set; }
        public WorkProgress AutomaticProgress { get; private set; }
        public int MaxWorkerSlots  => _slotsConfig.MaxSlots;
        public int UnlockedWorkerSlots => _unlockedWorkerSlots;

        public IManualWorker ManualWorker { get; private set; }
        public List<IAutomaticWorker> AutomaticWorkers { get; private set; }

        public void Initialize(IManualWorker manualWorker, List<IAutomaticWorker> automaticWorkers)
        {
            ManualWorker = manualWorker;
            AutomaticWorkers = automaticWorkers;
            _slotsConfig = _configProvider.GetGameConfig().WorkerSlotsConfig;
            _unlockedWorkerSlots = Mathf.Max(automaticWorkers.Count, _slotsConfig.InitialSlots);

            ManualProgress = new WorkProgress(0, ProgressType.Manual);
            ManualWorker.WorkDone += OnWorkDone;

            AutomaticProgress = new WorkProgress(0, ProgressType.Automatic);
            foreach (IAutomaticWorker automaticWorker in AutomaticWorkers)
            {
                automaticWorker.WorkDone += OnWorkDone;
            }

            _timeProvider.RegisterTimeListener(this);

            _isInitialized = true;
        }

        public void AddAutomaticWorker(IAutomaticWorker automaticWorker)
        {
            AutomaticWorkers.Add(automaticWorker);
            automaticWorker.WorkDone += OnWorkDone;

            AutomaticWorkersChanged?.Invoke();
        }

        public void OpenNewContractSlot()
        {
            _unlockedWorkerSlots = Math.Min(_unlockedWorkerSlots + 1, _slotsConfig.MaxSlots);
        }

        public void Dispose()
        {
            ManualWorker.WorkDone -= OnWorkDone;

            foreach (IAutomaticWorker automaticWorker in AutomaticWorkers)
            {
                automaticWorker.WorkDone -= OnWorkDone;
            }

            _timeProvider.ClearTimeListener(this);
        }

        public void Update(float deltaTime)
        {
            if (!_isInitialized)
            {
                return;
            }

            ManualWorker.Tick(deltaTime);

            foreach (IAutomaticWorker automaticWorker in AutomaticWorkers)
            {
                automaticWorker.Tick(deltaTime);
            }
        }

        private void OnWorkDone(float workAmount, ProgressType type)
        {
            switch (type)
            {
                case ProgressType.Manual:
                    ManualProgress.AddProgress(workAmount);
                    break;
                case ProgressType.Automatic:
                    AutomaticProgress.AddProgress(workAmount);
                    break;
            }
        }
    }
}