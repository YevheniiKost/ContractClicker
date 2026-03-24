using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class ContractModifierPreviewView : View
    {
        [SerializeField]
        private TextView _descriptionText;
        
        public void SetDescription(string description)
        {
            _descriptionText.SetText(description);
        }
    }
}