using System.Collections.Generic;
using System.Linq;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

namespace YeKostenko.ContractClicker.Domain.Contract
{
    public class Contract : IContract
    {
        private readonly List<IContractModifier> _modifiers = new List<IContractModifier>();

        public int Id { get; private set; }
        public string Name { get; private set; }

        public float RequiredProgress { get; private set; }
        public float CurrentProgress { get; private set; }

        public int Reward { get; private set; }

        public int FinalReward
        {
            get
            {
                int finalReward = Reward;
                foreach (var modifier in _modifiers.OfType<IRewardModifier>())
                {
                    if (modifier.IsActive)
                    {
                        finalReward = modifier.ModifyReward(finalReward);
                    }
                }
                return finalReward;
            }
        }

        public bool IsCompleted => CurrentProgress >= RequiredProgress;

        public IReadOnlyList<IContractModifier> Modifiers => _modifiers.AsReadOnly();

        public Contract(int id, string name, float requiredProgress, int reward)
        {
            Id = id;
            Name = name;
            RequiredProgress = requiredProgress;
            Reward = reward;
            CurrentProgress = 0f;
        }

        public void AddProgress(float amount, ProgressType progressType)
        {
            if (amount <= 0 || IsCompleted)
            {
                return;
            }

            // Створюємо контекст для модифікаторів
            var context = new ModifierContext(this, progressType, amount);

            // Обробляємо модифікатори за пріоритетом
            foreach (var modifier in _modifiers.OrderBy(m => m.Priority))
            {
                if (!modifier.IsActive)
                {
                    continue;
                }

                var result = modifier.Process(context);

                // Якщо невалідний - зупиняємо
                if (!result.IsValid)
                {
                    context.IsValid = false;
                    break;
                }

                // Якщо не потрібно продовжувати - зупиняємо
                if (!result.ShouldContinue)
                {
                    break;
                }
            }

            // Додаємо прогрес тільки якщо валідний
            if (context.IsValid)
            {
                CurrentProgress += context.ModifiedAmount;
            }
        }

        public void AddModifier(IContractModifier modifier)
        {
            if (modifier == null || _modifiers.Contains(modifier))
            {
                return;
            }

            _modifiers.Add(modifier);
            modifier.OnApply(this);
        }

        public void RemoveModifier(IContractModifier modifier)
        {
            if (modifier == null || !_modifiers.Contains(modifier))
            {
                return;
            }

            modifier.OnRemove(this);
            _modifiers.Remove(modifier);
        }

        public void RemoveModifier(string modifierId)
        {
            var modifier = _modifiers.FirstOrDefault(m => m.Id == modifierId);
            if (modifier != null)
            {
                RemoveModifier(modifier);
            }
        }

        public bool HasModifier<T>() where T : IContractModifier
        {
            return _modifiers.OfType<T>().Any();
        }

        public T GetModifier<T>() where T : IContractModifier
        {
            return _modifiers.OfType<T>().FirstOrDefault();
        }

        public void UpdateModifiers(float deltaTime)
        {
            foreach (var modifier in _modifiers)
            {
                if (modifier.IsActive)
                {
                    modifier.Update(deltaTime);
                }
            }
        }
    }
}