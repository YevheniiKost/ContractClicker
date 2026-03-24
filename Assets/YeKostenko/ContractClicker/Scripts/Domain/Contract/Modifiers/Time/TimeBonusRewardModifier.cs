using System;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time
{
    /// <summary>
    /// Модифікатор, що додає бонус до винагороди при виконанні у визначений час
    /// </summary>
    public class TimeBonusRewardModifier : ContractModifierBase, IRewardModifier
    {
        private readonly float _bonusTimeLimit; // час для бонусу в секундах
        private readonly float _bonusMultiplier;
        private float _elapsedTime;

        public float BonusTimeLimit => _bonusTimeLimit;
        public float BonusMultiplier => _bonusMultiplier;
        public float ElapsedTime => _elapsedTime;
        public bool IsInBonusTime => _elapsedTime <= _bonusTimeLimit;

        public event Action OnBonusExpired;

        public TimeBonusRewardModifier(float bonusTimeInSeconds, float bonusMultiplier)
        {
            _bonusTimeLimit = bonusTimeInSeconds;
            _bonusMultiplier = bonusMultiplier;
            _elapsedTime = 0f;
            Type = ModifierType.Time;
        }

        public override void OnApply(IContract contract)
        {
            base.OnApply(contract);
            _elapsedTime = 0f;
        }

        public override void Update(float deltaTime)
        {
            if (_elapsedTime > _bonusTimeLimit)
            {
                return;
            }

            _elapsedTime += deltaTime;

            if (_elapsedTime > _bonusTimeLimit)
            {
                OnBonusExpired?.Invoke();
            }
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Не впливає на процес додавання прогресу
            return ModifierResult.Continue();
        }

        public int ModifyReward(int baseReward)
        {
            // Якщо контракт виконано в межах бонусного часу - множимо винагороду
            if (AttachedContract != null && AttachedContract.IsCompleted && IsInBonusTime)
            {
                return (int)(baseReward * _bonusMultiplier);
            }

            return baseReward;
        }

        public override string ToString()
        {
            if (IsInBonusTime)
            {
                return $"Bonus x{_bonusMultiplier:F2} (Time: {(_bonusTimeLimit - _elapsedTime):F1}s)";
            }
            return $"Bonus Expired";
        }
    }
}

