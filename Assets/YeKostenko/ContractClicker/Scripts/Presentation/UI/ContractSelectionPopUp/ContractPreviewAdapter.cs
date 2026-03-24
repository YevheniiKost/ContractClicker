using System;
using System.Collections.Generic;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Data.Modifiers;
using YeKostenko.CoreKit.UI.Adapters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractPreviewAdapter : BaseListAdapter<ContractPreviewView, ContractDefinition>
    {
        public ContractPreviewAdapter(ContractPreviewView viewRef, Transform content) : base(viewRef, content)
        {
        }

        public Action<int> ContractClick { get; set; }

        protected override void BindView(int position, ContractPreviewView view, ContractDefinition data)
        {
            view.SetName(data.Name);
            view.SetReward(data.Reward);
            view.SetProgress(data.RequiredProgress);

            if (data.ModifiersSet != null)
            {
                view.SetBackgroundColor(data.ModifiersSet.DifficultyColor);
                List<string> modifiersNames = new List<string>();
                foreach (ModifierConfigBase config in data.ModifiersSet.GetActiveModifierConfigs())
                {
                    modifiersNames.Add(config.GetDescription());
                }
                view.SetModifiers(modifiersNames);
            }

            view.OnClick = () => ContractClick?.Invoke(data.Id);
        }
    }
}