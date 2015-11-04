namespace Alvasoft.KPPBridge.Common
{
    /// <summary>
    /// Значение для сохранения в БД.
    /// </summary>
    public class ValueParameter
    {
        /// <summary>
        /// Имя параметра в запросе.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение параметра.
        /// </summary>
        public double Value { get; set; }
    }
}
