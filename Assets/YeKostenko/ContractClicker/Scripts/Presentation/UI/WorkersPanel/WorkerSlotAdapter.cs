using System;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.CoreKit.UI.Adapters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    internal class WorkerSlotAdapter : BaseListAdapter<WorkerSlotView, WorkerSlotArgs>
    {
        public WorkerSlotAdapter(WorkerSlotView viewRef, Transform content) : base(viewRef, content)
        {
        }

        public Action<int> SlotClick { get; set; }

        protected override void BindView(int position, WorkerSlotView view, WorkerSlotArgs data)
        {
            ObjectSlotState state = ObjectSlotState.Empty;
            if (data.IsLocked)
            {
                state = ObjectSlotState.Locked;
            }
            else if (!data.IsEmpty)
            {
                state = ObjectSlotState.Active;
            }

            view.SetState(state);

            if (state == ObjectSlotState.Active)
            {
                view.SetLevel(data.Worker.Level);
                float yieldPerTick = data.Worker.GetStat(StatId.YieldPerTick, true, true);
                float tickDuration = data.Worker.GetStat(StatId.TickInterval, true, true);
                float yieldPerSecond = yieldPerTick / tickDuration;

                view.SetWorkText(yieldPerSecond);
            }

            view.SelfButtonClicked = () => SlotClick?.Invoke(data.SlotIndex);
        }
    }
}