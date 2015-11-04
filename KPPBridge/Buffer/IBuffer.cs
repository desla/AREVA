namespace Alvasoft.KPPBridge.Buffer
{
    using Common;

    /// <summary>
    /// Интерфейс буфера для хранения данных.
    /// </summary>
    public interface IBuffer
    {
        /// <summary>
        /// Добавляет данные в буфер.
        /// </summary>
        /// <param name="aPackage">Пакет данных.</param>
        void AddValues(ValuesPackage aPackage);

        /// <summary>
        /// Возвращает данные из буфера.
        /// </summary>
        /// <returns>Пакеты данных.</returns>
        ValuesPackage[] GetValues();

        /// <summary>
        /// Очищает буфер.
        /// </summary>
        void Clear();

        /// <summary>
        /// Возвращает состояние буфера.
        /// </summary>
        /// <returns>True - если буфер пуст, false - иначе.</returns>
        bool IsEmpty();
    }
}
