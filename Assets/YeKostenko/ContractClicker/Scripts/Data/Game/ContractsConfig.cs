using System.Collections.Generic;

using YeKostenko.CoreKit.Structs;

namespace YeKostenko.ContractClicker.Data.Game
{
    public class ContractsConfig
    {
        public ContractsConfig(HashSet<string> contractNames,
            float timeToComplete,
            float rewardMultiplier,
            float rewardPowerExponent,
            float easyContractMultiplier,
            float mediumContractMultiplier,
            float hardContractMultiplier,
            MinMaxValue timeToCompleteRange)
        {
            ContractNames = contractNames;
            TimeToComplete = timeToComplete;
            RewardMultiplier = rewardMultiplier;
            RewardPowerExponent = rewardPowerExponent;
            EasyContractMultiplier = easyContractMultiplier;
            MediumContractMultiplier = mediumContractMultiplier;
            HardContractMultiplier = hardContractMultiplier;
            TimeToCompleteRange = timeToCompleteRange;
        }

        public HashSet<string> ContractNames { get; }

        public float TimeToComplete { get; }
        public float RewardMultiplier { get; }
        public float RewardPowerExponent { get; }

        public float EasyContractMultiplier { get; }
        public float MediumContractMultiplier { get; }
        public float HardContractMultiplier { get; }

        public MinMaxValue TimeToCompleteRange { get; }
    }
}