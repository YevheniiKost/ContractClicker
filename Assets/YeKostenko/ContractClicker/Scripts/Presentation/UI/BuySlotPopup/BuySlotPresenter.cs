using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Economy;
using YeKostenko.ContractClicker.Domain.UseCases;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class BuySlotPresenter : IBuySlotPresenter
    {
        private readonly IPlayerWallet _playerWallet;
        private readonly IWorkController _workController;
        private readonly IContractProcessorModel _contractProcessorModel;
        private readonly ICalculateNextWorkerSlotPriceQuery _calculateNextWorkerSlotPriceQuery;
        private readonly ICalculateNextContractSlotPriceQuery _calculateNextContractSlotPriceQuery;

        private IBuySlotView _view;
        private BuySlotPopupType _popupType;

        public BuySlotPresenter(IWorkController workController,
            IContractProcessorModel contractProcessorModel,
            ICalculateNextWorkerSlotPriceQuery calculateNextWorkerSlotPriceQuery,
            ICalculateNextContractSlotPriceQuery calculateNextContractSlotPriceQuery,
            IPlayerWallet playerWallet)
        {
            _workController = workController;
            _contractProcessorModel = contractProcessorModel;
            _calculateNextWorkerSlotPriceQuery = calculateNextWorkerSlotPriceQuery;
            _calculateNextContractSlotPriceQuery = calculateNextContractSlotPriceQuery;
            _playerWallet = playerWallet;
        }

        public void AttachView(IBuySlotView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.BuyButtonClick += OnBuyButtonClick;
            _view.CloseButtonClick += OnCloseButtonClick;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnCreate;
                _view.BuyButtonClick -= OnBuyButtonClick;
                _view.CloseButtonClick -= OnCloseButtonClick;
            }

            _view = null;
        }

        private void OnCreate(BuySlotPopupType popupType)
        {
            _popupType = popupType;
            int price = GetPrice(popupType);
            _view.SetPrice($"Price: {price}");
            _view.SetBuyButtonInteractable(price <= _playerWallet.GetBalance(CurrencyType.Gold));
            _view.SetTitleText(GetTitle(popupType));
        }

        private void OnBuyButtonClick()
        {
            int price = GetPrice(_popupType);
            if (_playerWallet.TrySpendFunds(CurrencyType.Gold, price))
            {
                switch (_popupType)
                {
                    case BuySlotPopupType.Worker:
                        _workController.OpenNewContractSlot();
                        break;
                    case BuySlotPopupType.Contract:
                        _contractProcessorModel.OpenNewContractSlot();
                        break;
                }

                _view.Close();
            }
        }

        private void OnCloseButtonClick()
        {
            _view.Close();
        }

        private int GetPrice(BuySlotPopupType popupType)
        {
            return popupType switch
            {
                BuySlotPopupType.Worker => _calculateNextWorkerSlotPriceQuery.Execute(),
                BuySlotPopupType.Contract => _calculateNextContractSlotPriceQuery.Execute(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(popupType), popupType, null)
            };
        }

        private string GetTitle(BuySlotPopupType popupType)
        {
            return popupType switch
            {
                BuySlotPopupType.Worker => "Buy Worker Slot",
                BuySlotPopupType.Contract => "Buy Contract Slot",
                _ => throw new System.ArgumentOutOfRangeException(nameof(popupType), popupType, null)
            };
        }
    }
}