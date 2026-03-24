using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public class WorkerStatsView : View
    {
        [SerializeField]
        private TextView _yieldPerTickText;
        [SerializeField]
        private TextView _critChanceText;
        [SerializeField]
        private TextView _critMultiplierText;
        [SerializeField]
        private TextView _tickIntervalText;

        public void SetYieldPerTickText(float value)
        {
            if (_yieldPerTickText != null)
            {
                _yieldPerTickText.SetText(value.ToStatString());
            }
        }

        public void SetCritChanceText(float value)
        {
            if (_critChanceText != null)
            {
                _critChanceText.SetText(value.ToPercentageString());
            }
        }

        public void SetCritMultiplierText(float value)
        {
            if (_critMultiplierText != null)
            {
                _critMultiplierText.SetText(value.ToStatString());
            }
        }

        public void SetTickInterval(float value)
        {
            if (_tickIntervalText != null)
            {
                _tickIntervalText.SetText(value.ToStatString());
            }
        }
    }
}