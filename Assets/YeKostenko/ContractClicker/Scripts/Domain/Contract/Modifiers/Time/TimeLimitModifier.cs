using System;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time
{
    /// <summary>
    /// Модифікатор з обмеженням часу виконання
    /// </summary>
    public class TimeLimitModifier : ContractModifierBase
    {
        private readonly float _timeLimit; // в секундах
        private float _elapsedTime;
        private bool _isExpired;

        public float TimeLimit => _timeLimit;
        public float ElapsedTime => _elapsedTime;
        public float RemainingTime => Math.Max(0, _timeLimit - _elapsedTime);
        public bool IsExpired => _isExpired;

        public event Action OnExpired;

        public TimeLimitModifier(float timeLimitInSeconds)
        {
            _timeLimit = timeLimitInSeconds;
            _elapsedTime = 0f;
            _isExpired = false;
            Type = ModifierType.Time;
        }

        public override void OnApply(IContract contract)
        {
            base.OnApply(contract);
            _elapsedTime = 0f;
            _isExpired = false;
        }

        public override void Update(float deltaTime)
        {
            if (_isExpired)
            {
                return;
            }

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _timeLimit)
            {
                _isExpired = true;
                Deactivate();
                OnExpired?.Invoke();
            }
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Якщо час вичерпано - не дозволяємо додавати прогрес
            if (_isExpired)
            {
                return ModifierResult.Invalid("Time limit exceeded");
            }

            return ModifierResult.Continue();
        }

        public override string ToString()
        {
            return $"Time Limit: {RemainingTime:F1}s";
        }
    }
}

