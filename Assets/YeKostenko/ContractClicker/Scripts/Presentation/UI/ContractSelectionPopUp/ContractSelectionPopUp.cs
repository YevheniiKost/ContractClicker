using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.CoreKit.DI;
using YeKostenko.CoreKit.UI;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractSelectionPopUp : ContractClickerWindow, IContractSelectionView
    {
        [SerializeField]
        private ButtonView _closeButton;
        [SerializeField]
        private Transform _previewContainer;

        [Header("Resources References")]
        [SerializeField]
        private ContractPreviewView _previewViewRef;

        private IContractSelectionPresenter _presenter;
        private ContractPreviewAdapter _adapter;
        private ContractSelectionUIContext _context;

        public event Action Create;
        public event Action CloseClick;
        public event Action<int> ContractSelected;

        [Inject]
        public void Construct(IContractSelectionPresenter contractPanelPresenter)
        {
            _presenter = contractPanelPresenter;

            _presenter.AttachView(this);
        }

        public void SetContracts(List<ContractDefinition> contracts)
        {
            _adapter.SetData(contracts);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_previewContainer.transform as RectTransform);
        }

        public void Close(ContractDefinition contractDefinition)
        {
            if (contractDefinition != null)
            {
                _context.OnContractSelected?.Invoke(contractDefinition);
            }

            CloseView();
        }

        protected override void OnCreateWindow(IUIContext context)
        {
            _context = context as ContractSelectionUIContext? ??
                       throw new ArgumentException(
                           $"Expected context of type {typeof(ContractSelectionUIContext)}, but got {context.GetType()}");

            _adapter = new ContractPreviewAdapter(_previewViewRef, _previewContainer)
            {
                ContractClick = id => ContractSelected?.Invoke(id)
            };
            _closeButton.ButtonClicked = () => CloseClick?.Invoke();

            Create?.Invoke();
        }

        protected override void OnDestroyWindow()
        {
            _presenter.DetachView();
            _adapter.ContractClick = null;
            _closeButton.ButtonClicked = null;
            _presenter = null;
            _adapter = null;

            base.OnDestroyWindow();
        }
    }
}