using Cysharp.Threading.Tasks;

using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Data.Game
{
    public class GameConfigProvider : IGameConfigProvider
    {
        private GameConfigInternal _gameConfigInternal;
        private ModifierDatabase _modifierDatabase;

        public async UniTask Initialize()
        {
            _gameConfigInternal = await GameConfigInternal.Load();
            _modifierDatabase = await ModifierDatabase.Load();
        }

        public IGameConfig GetGameConfig() => _gameConfigInternal;
        public IModifierDatabase GetModifierDatabase() => _modifierDatabase;
    }
}