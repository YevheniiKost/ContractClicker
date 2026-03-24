namespace YeKostenko.ContractClicker.Presentation.UI
{
    public static class TextExtensions
    {
        public static string ToStatString(this float value)
        {
            if (value > 100)
            {
                return $"{value:F1}%";
            }

            return $"{value:F2}";
        }

        public static string ToPercentageString(this float value)
        {
            return $"{value * 100:F1}%";
        }
    }
}