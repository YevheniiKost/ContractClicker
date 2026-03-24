using System.Collections.Generic;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Contract;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractSelectionPresenter : IContractSelectionPresenter
    {
        private const int ContractsToGenerate = 3;

        private readonly IContractGenerator _contractGenerator;

        private IContractSelectionView _view;
        private List<ContractDefinition> _contracts;
        private ContractDefinition _selectedContract;

        public ContractSelectionPresenter(IContractGenerator contractGenerator)
        {
            _contractGenerator = contractGenerator;
        }

        public void AttachView(IContractSelectionView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.CloseClick += OnCloseClick;
            _view.ContractSelected += OnContractSelected;
        }

        public void DetachView()
        {
            if (_view == null)
            {
                return;
            }

            _view.Create -= OnCreate;
            _view.CloseClick -= OnCloseClick;
            _view.ContractSelected -= OnContractSelected;

            _view = null;
        }

        private void OnCreate()
        {
            _contracts = GenerateContracts(ContractsToGenerate);
            _view.SetContracts(_contracts);
        }

        private void OnCloseClick()
        {
            _view.Close(null);
        }

        private void OnContractSelected(int id)
        {
            ContractDefinition selectedContract = _contracts.Find(contract => contract.Id == id);
            if (selectedContract != null)
            {
                _selectedContract = selectedContract;
                _view.Close(_selectedContract);
            }
        }

        private List<ContractDefinition> GenerateContracts(int count)
        {
            return _contractGenerator.GenerateContracts(count);
        }
    }
}