using System;

using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterExpiredEventArgs : EventArgs
    {
        public BoosterExpiredEventArgs(BoosterConfig config)
        {
            Config = config;
        }

        public BoosterConfig Config { get; }
    }
}

