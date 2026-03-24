using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.CoreKit.UI.Adapters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    internal class ContractSlotsAdapter : BaseListAdapter<ContractSlotView, ContractSlotArgs>
    {
        private Dictionary<int, ContractSlotView> _contractSlotsAdapter;

        public ContractSlotsAdapter(ContractSlotView viewRef, Transform content) : base(viewRef, content)
        {
            _contractSlotsAdapter = new Dictionary<int, ContractSlotView>();
        }

        public Action<int> ContractClicked { get; set; }
        public Action<int> FillButtonClicked { get; set; }
        public Action<int, bool> AutoFillToggleChanged { get; set; }

        public Vector3 GetFillButtonPosition(int contractId)
        {
            if (_contractSlotsAdapter.TryGetValue(contractId, out ContractSlotView slotView))
            {
                return slotView.FillButtonPosition;
            }

            return Vector3.zero;
        }

        protected override void BindView(int position, ContractSlotView view, ContractSlotArgs data)
        {
            ObjectSlotState state = ObjectSlotState.Empty;
            if (data.IsLocked)
            {
                state = ObjectSlotState.Locked;
            }
            else if (!data.IsEmpty)
            {
                state = data.Contract.IsCompleted ? ObjectSlotState.Completed : ObjectSlotState.Active;
            }

            view.SetState(state);

            if (state == ObjectSlotState.Active)
            {
                IContract contract = data.Contract;
                view.SetProgress(contract.CurrentProgress, contract.RequiredProgress);
                view.SetContractName(contract.Name);
                view.SetAutoFillToggle(data.IsAutoFill);
            }

            view.SelfButtonClicked = () => ContractClicked?.Invoke(data.Id);
            view.FillButtonClicked = () => FillButtonClicked?.Invoke(data.Id);
            view.AutoFillToggleChanged = isOn => AutoFillToggleChanged?.Invoke(data.Id, isOn);

            _contractSlotsAdapter[data.Id] = view;
        }
    }
}