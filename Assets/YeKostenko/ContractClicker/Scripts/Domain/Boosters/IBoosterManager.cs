using System;
using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public interface IBoosterManager : IDisposable
    {
        event EventHandler<BoosterActivatedEventArgs> BoosterActivated;
        event EventHandler<BoosterExpiredEventArgs> BoosterExpired;

        IReadOnlyList<ActiveBooster> ActiveBoosters { get; }

        void ActivateBooster(BoosterConfig config);
    }
}

