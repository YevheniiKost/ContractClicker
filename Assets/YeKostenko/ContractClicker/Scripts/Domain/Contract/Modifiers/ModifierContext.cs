using System.Collections.Generic;

using YeKostenko.ContractClicker.Data;

namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    /// <summary>
    /// Контекст, що передається через ланцюжок модифікаторів
    /// </summary>
    public class ModifierContext
    {
        public IContract Contract { get; }
        public ProgressType ProgressType { get; }
        public float OriginalAmount { get; }
        public float ModifiedAmount { get; set; }
        public bool IsValid { get; set; }
        public Dictionary<string, object> Metadata { get; }

        public ModifierContext(IContract contract, ProgressType progressType, float amount)
        {
            Contract = contract;
            ProgressType = progressType;
            OriginalAmount = amount;
            ModifiedAmount = amount;
            IsValid = true;
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// Отримати метадані з приведенням типу
        /// </summary>
        public T GetMetadata<T>(string key, T defaultValue = default)
        {
            if (Metadata.TryGetValue(key, out var value) && value is T typed)
            {
                return typed;
            }
            return defaultValue;
        }

        /// <summary>
        /// Встановити метадані
        /// </summary>
        public void SetMetadata(string key, object value)
        {
            Metadata[key] = value;
        }
    }
}

