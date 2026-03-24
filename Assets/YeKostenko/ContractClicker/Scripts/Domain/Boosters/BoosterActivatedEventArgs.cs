using System;

using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class BoosterActivatedEventArgs : EventArgs
    {
        public BoosterActivatedEventArgs(BoosterConfig config, ActiveBooster activeBooster)
        {
            Config = config;
            ActiveBooster = activeBooster;
        }

        public BoosterConfig Config { get; }
        public ActiveBooster ActiveBooster { get; }
    }
}

