namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    /// <summary>
    /// Результат обробки модифікатора
    /// </summary>
    public struct ModifierResult
    {
        public bool ShouldContinue { get; }
        public bool IsValid { get; }
        public string Message { get; }

        private ModifierResult(bool shouldContinue, bool isValid, string message = null)
        {
            ShouldContinue = shouldContinue;
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// Продовжити обробку наступними модифікаторами
        /// </summary>
        public static ModifierResult Continue() => new ModifierResult(true, true);

        /// <summary>
        /// Зупинити обробку, але вважати валідним
        /// </summary>
        public static ModifierResult Stop(string message = null) => new ModifierResult(false, true, message);

        /// <summary>
        /// Позначити як невалідний (прогрес не буде додано)
        /// </summary>
        public static ModifierResult Invalid(string message = null) => new ModifierResult(false, false, message);

        /// <summary>
        /// Позначити як невалідний і продовжити (для логування)
        /// </summary>
        public static ModifierResult InvalidButContinue(string message = null) => new ModifierResult(true, false, message);
    }
}

