using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.CoreKit.Logging;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractCompletedPresenter : IContractCompletedPresenter
    {
        private readonly IContractProcessorModel _contractProcessorModel;
        private readonly IPlayerWallet _wallet;

        private IContractCompletedView _view;
        private IContract _completedContract;

        public ContractCompletedPresenter(IContractProcessorModel contractProcessorModel, IPlayerWallet wallet)
        {
            _contractProcessorModel = contractProcessorModel;
            _wallet = wallet;
        }

        public void AttachView(IContractCompletedView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnViewCreate;
            _view.CloseButtonClick += OnCloseButtonClick;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnViewCreate;
                _view.CloseButtonClick -= OnCloseButtonClick;
            }

            _view = null;
        }

        private void OnViewCreate(int contractId)
        {
            _completedContract = _contractProcessorModel.ActiveContracts.Find(x => x.Id == contractId);
            if (_completedContract == null)
            {
                Logger.LogError($"Contract with id {contractId} not found.");
                _view.Close();
                return;
            }

            if (!_completedContract.IsCompleted)
            {
                Logger.LogError($"Contract with id {contractId} is not completed.");
                _completedContract = null;
                _view.Close();
                return;
            }

            _view.SetTitle($"Contract {_completedContract.Name} Completed!");
            _view.SetRewardText($"{_completedContract.FinalReward}");
        }

        private void OnCloseButtonClick()
        {
            CompleteContract();
            _view.Close();
        }

        private void CompleteContract()
        {
            if (_completedContract != null)
            {
                int reward = _completedContract.FinalReward;
                _wallet.AddFunds(CurrencyType.Gold, reward);
                _contractProcessorModel.ClearContract(_completedContract.Id);
                _completedContract = null;
            }
        }
    }
}