using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class TimerView : View
    {
        private const string DefaultDisplayFormat = "{0:00}:{1:00}";

        [SerializeField]
        private TextView _timerText;

        [SerializeField]
        [Tooltip("Format tokens: {0} = minutes, {1} = seconds, {2} = total seconds. Example: {0:00}:{1:00}")]
        private string _displayFormat = DefaultDisplayFormat;

        private float _remainingSeconds;
        private bool _isRunning;

        public event EventHandler TimerExpired;

        public void StartTimer(float totalSeconds)
        {
            _remainingSeconds = Mathf.Max(0f, totalSeconds);
            _isRunning = true;
            UpdateDisplay();
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            _remainingSeconds -= Time.deltaTime;

            if (_remainingSeconds <= 0f)
            {
                _remainingSeconds = 0f;
                _isRunning = false;
                UpdateDisplay();
                TimerExpired?.Invoke(this, EventArgs.Empty);
                return;
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            int totalSeconds = Mathf.CeilToInt(_remainingSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            string format = string.IsNullOrEmpty(_displayFormat) ? DefaultDisplayFormat : _displayFormat;
            _timerText.SetText(string.Format(format, minutes, seconds, totalSeconds));
        }
    }
}
