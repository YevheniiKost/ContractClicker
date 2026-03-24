using System.Collections.Generic;

namespace YeKostenko.ContractClicker.Domain.Economy
{
    public class PlayerWalletFactory : IPlayerWalletFactory
    {
        public IPlayerWallet Create() => new PlayerWallet();
    }
}