using UnityEngine;

using YeKostenko.CoreKit.UI.Adapters;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractModifierPreviewAdapter : BaseListAdapter<ContractModifierPreviewView, string>
    {
        public ContractModifierPreviewAdapter(ContractModifierPreviewView viewRef, Transform content) : base(viewRef, content)
        {
        }

        protected override void BindView(int position, ContractModifierPreviewView view, string data)
        {
            view.SetDescription(data);
        }
    }
}