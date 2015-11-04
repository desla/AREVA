namespace Alvasoft.KPPBridge.Common
{
    using System;

    /// <summary>
    /// Пакет данных для передачи в ИТС.
    /// </summary>
    public class ValuesPackage
    {
        /// <summary>
        /// Идентификатор серии.
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// Время.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Параметры для сохранения.
        /// </summary>
        public ValueParameter[] Parameters { get; set; }
    }
}
