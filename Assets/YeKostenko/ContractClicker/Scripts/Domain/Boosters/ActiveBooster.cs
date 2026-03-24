using YeKostenko.ContractClicker.Data.Boosters;

namespace YeKostenko.ContractClicker.Domain.Boosters
{
    public class ActiveBooster
    {
        public ActiveBooster(BoosterConfig config)
        {
            Config = config;
            RemainingTimeInSeconds = config.DurationInSeconds;
        }

        public BoosterConfig Config { get; }
        public float RemainingTimeInSeconds { get; private set; }

        public bool Tick(float deltaTime)
        {
            RemainingTimeInSeconds -= deltaTime;
            return RemainingTimeInSeconds <= 0f;
        }
    }
}

