using System.Collections.Generic;
using System.Linq;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain;
using YeKostenko.ContractClicker.Domain.Automation;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Work;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractPanelPresenter : IContractPanelPresenter
    {
        private readonly IContractProcessorModel _contractProcessorModel;
        private readonly IWorkController _workController;
        private readonly IContractFactory _contractFactory;
        private readonly IAutoFillModel _autoFillModel;

        private IContractPanelView _view;
        private Dictionary<int, ContractSlotArgs> _contractSlots = new Dictionary<int, ContractSlotArgs>();

        public ContractPanelPresenter(IContractProcessorModel contractProcessorModel,
            IWorkController workController,
            IContractFactory contractFactory,
            IAutoFillModel autoFillModel)
        {
            _contractProcessorModel = contractProcessorModel;
            _workController = workController;
            _contractFactory = contractFactory;
            _autoFillModel = autoFillModel;
        }

        public void AttachView(IContractPanelView view)
        {
            _view = view ?? throw new System.ArgumentNullException(nameof(view));

            _view.Create += OnCreate;
            _view.SlotClicked += OnSlotClicked;
            _view.FillContractClicked += OnFillContractClicked;
            _view.AutoFillToggled += OnAutoFillToggled;

            _contractProcessorModel.NewContractStarted += OnNewContractStarted;
            _contractProcessorModel.ContractCompleted += OnContractCompleted;
            _contractProcessorModel.ProgressAdded += OnProgressAdded;
            _contractProcessorModel.ContractSkipped += OnContractSkipped;

            _autoFillModel.AutoFillChanged += OnAutoFillChanged;
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.Create -= OnCreate;
                _view.SlotClicked -= OnSlotClicked;
                _view.FillContractClicked -= OnFillContractClicked;
                _view.AutoFillToggled -= OnAutoFillToggled;
            }

            _contractProcessorModel.NewContractStarted -= OnNewContractStarted;
            _contractProcessorModel.ContractCompleted -= OnContractCompleted;
            _contractProcessorModel.ProgressAdded -= OnProgressAdded;
            _contractProcessorModel.ContractSkipped -= OnContractSkipped;

            _autoFillModel.AutoFillChanged -= OnAutoFillChanged;

            _view = null;
        }


        private void OnFillContractClicked(int slotId)
        {
            if (_contractSlots.TryGetValue(slotId, out ContractSlotArgs args) && !args.IsEmpty)
            {
                IContract contract = args.Contract;
                float progressNeeded = contract.RequiredProgress - contract.CurrentProgress;
                float manualProgress = CanAddProgress(contract, ProgressType.Manual) ? _workController.ManualProgress.Amount : 0;
                float automaticProgress = CanAddProgress(contract, ProgressType.Automatic) ? _workController.AutomaticProgress.Amount : 0;
                float allProgress = manualProgress + automaticProgress;
                //TODO check if contract can be completed with both type of progress;


                float progressToShow = System.Math.Min(progressNeeded, allProgress);

                if (allProgress <= 0)
                {
                     //TODO show notification that there is no progress to add
                    return;
                }

                _view.OpenFillContractWidget(slotId, progressToShow, (selectedProgress) =>
                {
                    float progressToAdd = System.Math.Min(progressToShow, selectedProgress);

                    float progressLeftToAdd = progressToAdd;
                    float manualProgressToSpend = System.Math.Min(manualProgress, progressToAdd / 2);
                    progressLeftToAdd -= manualProgressToSpend;
                    float automaticProgressToSpend = System.Math.Min(automaticProgress, progressLeftToAdd);
                    progressLeftToAdd -= automaticProgressToSpend;
                    if(progressLeftToAdd > 0)
                    {
                        //If there is still some progress left to add, try to spend it from the other type of progress
                        float additionalManualProgress = System.Math.Min(manualProgress - manualProgressToSpend, progressLeftToAdd);
                        manualProgressToSpend += additionalManualProgress;
                        progressLeftToAdd -= additionalManualProgress;

                        float additionalAutomaticProgress = System.Math.Min(automaticProgress - automaticProgressToSpend, progressLeftToAdd);
                        automaticProgressToSpend += additionalAutomaticProgress;
                    }

                    _workController.ManualProgress.SpendProgress(manualProgressToSpend);
                    _workController.AutomaticProgress.SpendProgress(automaticProgressToSpend);

                    _contractProcessorModel.AddProgressToContract(contract.Id, manualProgressToSpend, ProgressType.Manual);
                    _contractProcessorModel.AddProgressToContract(contract.Id, automaticProgressToSpend, ProgressType.Automatic);
                });
            }
        }

        private void OnAutoFillToggled(int slotId, bool isAutoFill)
        {
            if (_contractSlots.TryGetValue(slotId, out ContractSlotArgs args) && !args.IsEmpty)
            {
                _autoFillModel.SetContractAutoFill(args.Contract.Id, isAutoFill);
            }
        }

        private void OnCreate()
        {
            UpdateContractSlots();
        }

        private void OnAutoFillChanged(int id)
        {
            UpdateContractSlots();
        }

        private void OnSlotClicked(int slotId)
        {
            if (_contractSlots.TryGetValue(slotId, out ContractSlotArgs args))
            {
                if (args.IsEmpty)
                {
                    if (args.IsLocked)
                    {
                        _view.OpenBuyContractSlotPopup(UpdateContractSlots);
                    }
                    else
                    {
                        _view.OpenContractSelectionPopUp(SelectContract);
                    }
                }
                else
                {
                    IContract contract = args.Contract;
                    if (contract.IsCompleted)
                    {
                        _view.OpenContractCompletePopUp(contract.Id, UpdateContractSlots);
                    }
                    else
                    {
                        _view.OpenContractInfoPopUp(contract.Id, UpdateContractSlots);
                    }
                }
            }
        }

        private void SelectContract(ContractDefinition definition)
        {
            IContract contract = _contractFactory.Create(definition);
            _contractProcessorModel.StartNewContract(contract);
        }

        private void OnNewContractStarted(IContract obj)
        {
            //TODO: Animation or notification
            UpdateContractSlots();
        }

        private void OnContractCompleted(IContract obj)
        {
            //TODO: Animation or notification
            UpdateContractSlots();
        }

        private void OnProgressAdded(IContract obj)
        {
            UpdateContractSlots();
        }

        private void OnContractSkipped(IContract obj)
        {
            UpdateContractSlots();
        }

        private void UpdateContractSlots()
        {
            List<IContract> activeContracts = _contractProcessorModel.ActiveContracts;
            int availableSlots = _contractProcessorModel.UnlockedContractSlots;
            int maxSlots = _contractProcessorModel.MaxContractSlots;
            int activeCount = activeContracts.Count;

            for (int i = 0; i < maxSlots; i++)
            {
                int slotId = i + 1;
                ContractSlotArgs args;

                if (i < activeCount)
                {
                    IContract contract = activeContracts[i];
                    args = ContractSlotArgs.WithContract(slotId, contract, _autoFillModel.IsContractAutoFillEnabled(contract.Id));
                }
                else
                {
                    args = ContractSlotArgs.Empty(slotId, i >= availableSlots);
                }

                _contractSlots[slotId] = args;
            }

            _view.SetContractsData(_contractSlots.Values.ToList());
        }

        private bool CanAddProgress(IContract contract, ProgressType progressType)
        {
            bool result = true;

            if (contract.Modifiers.Count == 0)
            {
                return result;
            }

            foreach (IContractModifier modifier in contract.Modifiers)
            {
                if (modifier is WorkTypeRestrictionModifier restrictionModifier &&
                    restrictionModifier.AllowedType != progressType)
                {
                     result = false;
                     break;
                }
            }

            return result;
        }
    }
}