using Cysharp.Threading.Tasks;
using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Data.Game
{
    public interface IGameConfigProvider
    {
        UniTask Initialize();
        IGameConfig GetGameConfig();
        IModifierDatabase GetModifierDatabase();
    }
}