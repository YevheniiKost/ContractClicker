using System;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special
{
    /// <summary>
    /// Модифікатор комбо - збільшує бонус при послідовному додаванні прогресу
    /// </summary>
    public class ComboProgressModifier : ContractModifierBase
    {
        private readonly int _maxCombo;
        private readonly float _comboBonus; // бонус за кожний рівень комбо
        private readonly float _comboResetTime; // час, за який комбо скидається

        private int _currentCombo;
        private float _timeSinceLastProgress;

        public int MaxCombo => _maxCombo;
        public int CurrentCombo => _currentCombo;
        public float ComboBonus => _comboBonus;
        public float TimeSinceLastProgress => _timeSinceLastProgress;

        public event Action<int> OnComboChanged;

        public ComboProgressModifier(int maxCombo = 10, float comboBonus = 0.1f, float comboResetTime = 3f)
        {
            _maxCombo = maxCombo;
            _comboBonus = comboBonus;
            _comboResetTime = comboResetTime;
            _currentCombo = 0;
            _timeSinceLastProgress = 0f;
            Type = ModifierType.Special;
        }

        public override void OnApply(IContract contract)
        {
            base.OnApply(contract);
            _currentCombo = 0;
            _timeSinceLastProgress = 0f;
        }

        public override void Update(float deltaTime)
        {
            _timeSinceLastProgress += deltaTime;

            // Скидаємо комбо якщо пройшло занадто багато часу
            if (_timeSinceLastProgress >= _comboResetTime && _currentCombo > 0)
            {
                ResetCombo();
            }
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Збільшуємо комбо
            if (_timeSinceLastProgress < _comboResetTime)
            {
                _currentCombo = Math.Min(_currentCombo + 1, _maxCombo);
            }
            else
            {
                _currentCombo = 1;
            }

            // Скидаємо таймер
            _timeSinceLastProgress = 0f;

            // Застосовуємо бонус від комбо
            float comboMultiplier = 1f + (_currentCombo - 1) * _comboBonus;
            context.ModifiedAmount *= comboMultiplier;

            // Зберігаємо інформацію про комбо в метадані
            context.SetMetadata("ComboLevel", _currentCombo);
            context.SetMetadata("ComboMultiplier", comboMultiplier);

            OnComboChanged?.Invoke(_currentCombo);

            return ModifierResult.Continue();
        }

        private void ResetCombo()
        {
            _currentCombo = 0;
            OnComboChanged?.Invoke(_currentCombo);
        }

        public override string ToString()
        {
            return $"Combo x{_currentCombo} (Bonus: {(_currentCombo * _comboBonus * 100):F0}%)";
        }
    }
}

