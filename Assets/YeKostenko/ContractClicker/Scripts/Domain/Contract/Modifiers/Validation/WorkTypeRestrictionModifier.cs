using YeKostenko.ContractClicker.Data;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation
{
    /// <summary>
    /// Модифікатор, що обмежує тип роботи (тільки Manual або тільки Auto)
    /// </summary>
    public class WorkTypeRestrictionModifier : ContractModifierBase
    {
        private readonly ProgressType _allowedType;

        public ProgressType AllowedType => _allowedType;

        public WorkTypeRestrictionModifier(ProgressType allowedType)
        {
            _allowedType = allowedType;
            Type = ModifierType.Validation;
        }

        public override ModifierResult Process(ModifierContext context)
        {
            // Перевіряємо чи відповідає тип роботи дозволеному
            if (context.ProgressType != _allowedType)
            {
                return ModifierResult.Invalid($"Only {_allowedType} work is allowed");
            }

            return ModifierResult.Continue();
        }

        public override string ToString()
        {
            return $"{_allowedType} Only";
        }
    }
}

