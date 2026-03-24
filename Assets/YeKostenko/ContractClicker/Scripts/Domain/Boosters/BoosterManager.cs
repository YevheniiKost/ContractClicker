using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Boosters;

using YevheniiKostenko.CoreKit.Time;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterManager : IBoosterManager, ITimeListener
    {
        private readonly ITimeProvider _timeProvider;
        private readonly List<ActiveBooster> _activeBoosters = new List<ActiveBooster>();

        public BoosterManager(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _timeProvider.RegisterTimeListener(this);
        }

        public event EventHandler<BoosterActivatedEventArgs> BoosterActivated;
        public event EventHandler<BoosterExpiredEventArgs> BoosterExpired;

        public IReadOnlyList<ActiveBooster> ActiveBoosters => _activeBoosters;

        public void ActivateBooster(BoosterConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            ActiveBooster activeBooster = new ActiveBooster(config);
            _activeBoosters.Add(activeBooster);

            BoosterActivated?.Invoke(this, new BoosterActivatedEventArgs(config, activeBooster));
        }

        public void Update(float deltaTime)
        {
            for (int i = _activeBoosters.Count - 1; i >= 0; i--)
            {
                bool hasExpired = _activeBoosters[i].Tick(deltaTime);

                if (hasExpired)
                {
                    BoosterConfig expiredConfig = _activeBoosters[i].Config;
                    _activeBoosters.RemoveAt(i);
                    BoosterExpired?.Invoke(this, new BoosterExpiredEventArgs(expiredConfig));
                }
            }
        }

        public void Dispose()
        {
            _timeProvider.ClearTimeListener(this);
        }
    }
}

