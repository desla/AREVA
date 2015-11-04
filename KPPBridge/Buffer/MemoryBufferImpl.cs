namespace Alvasoft.KPPBridge.Buffer
{
    using System;
    using System.Collections.Generic;
    using Common;
    using log4net;

    /// <summary>
    /// Реализация буфера в памяти.
    /// </summary>
    public class MemoryBufferImpl : IBuffer
    {
        private static readonly ILog logger = LogManager.GetLogger("MemoryBufferImpl");

        /// <summary>
        /// Ограничение на количество записей.
        /// </summary>
        private const int MAX_SIZE = 1000000;

        /// <summary>
        /// Набор для хранения данных.
        /// </summary>
        private List<ValuesPackage> packages = new List<ValuesPackage>();

        public void AddValues(ValuesPackage aPackage)
        {
            if (aPackage == null) {
                throw new ArgumentNullException("aPackage");
            }

            lock (packages) {
                if (packages.Count < MAX_SIZE) {
                    packages.Add(aPackage);
                }
                else {
                    logger.Error("Превышено максимальное число записей в буфере.");
                }
            }
        }

        public ValuesPackage[] GetValues()
        {
            lock (packages) {
                return packages.ToArray();
            }
        }

        public void Clear()
        {
            lock (packages) {
                packages.Clear();
            }
        }

        public bool IsEmpty()
        {
            return packages.Count == 0;
        }
    }
}
