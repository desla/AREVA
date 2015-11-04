namespace Alvasoft.KPPBridge.ITS
{
    using System;
    using Common;
    using Configuration;
    using ConnectionHolders;
    using Utils.Activity;

    /// <summary>
    /// Интерфейс для передачи данных в ИТС.
    /// </summary>
    public interface IIts : IInitializable
    {
        /// <summary>
        /// Устанавливает держатель соединения.
        /// </summary>
        /// <param name="aConnectionHolder">Держатель соединения.</param>
        void SetConnectionHoder(OracleConnectionHolder aConnectionHolder);

        /// <summary>
        /// Устанавливает конфигурацию серий.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация серий.</param>
        void SetLinesConfiguration(LinesConfiguration aConfiguration);

        /// <summary>
        /// Делает попытку записать параметр в БД.
        /// </summary>        
        /// <param name="aPackage">Пакет данных для передачи в ИТС.</param>
        /// <returns>True - если запись прошла успешно, false - иначе.</returns>
        bool TryWritePackage(ValuesPackage aPackage);

        /// <summary>
        /// Делает попытку записать параметры в БД.
        /// </summary>        
        /// <param name="aPackages">Пакеты данных для передачи в ИТС.</param>
        /// <returns>True - если запись прошла успешно, false - иначе.</returns>
        bool TryWritePackages(ValuesPackage[] aPackages);
    }
}
