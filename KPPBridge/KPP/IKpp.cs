namespace Alvasoft.KPPBridge.KPP
{
    using Common;
    using Configuration;
    using ConnectionHolders;
    using Utils.Activity;

    /// <summary>
    /// Интерфейс для считывания данных из КПП.
    /// </summary>
    public interface IKpp : IInitializable
    {
        /// <summary>
        /// Устанавливает держатель соединенния.
        /// </summary>
        /// <param name="aConnectionHolder">Держатель соединения.</param>
        void SetConnectionHolder(OpcConnectionHolder aConnectionHolder);

        /// <summary>
        /// Устанавливает имена ОРС-тегов.
        /// </summary>
        /// <param name="aParameters">Имена ОРС-тегов.</param>
        void SetParametersNames(QueryParameter[] aParameters);

        /// <summary>
        /// Возвращает значение параметра по имени.
        /// </summary>
        /// <param name="aParameter">Параметра.</param>
        /// <returns>Значение параметра.</returns>
        ValueParameter GetValue(QueryParameter aParameter);
    }
}
