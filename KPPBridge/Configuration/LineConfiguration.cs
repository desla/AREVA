namespace Alvasoft.KPPBridge.Configuration
{
    /// <summary>
    /// Конфигурация серии.
    /// </summary>
    public class LineConfiguration
    {
        /// <summary>
        /// Идентификатор серии.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Интервал забора данных, начиная от начала суток.
        /// </summary>
        public int Interval { get; set; }        

        /// <summary>
        /// Список ОРС-тегов - параметры для запроса.
        /// </summary>
        public QueryParameter[] Parameters { get; set; }
    }
}
