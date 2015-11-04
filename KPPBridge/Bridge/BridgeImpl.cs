namespace Alvasoft.KPPBridge.Bridge
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Timers;
    using System.Windows.Forms;
    using Buffer;
    using Common;
    using Configuration;
    using ConnectionHolders;
    using ITS;
    using KPP;
    using Utils.Activity;
    using log4net;
    using OPCAutomation;
    using Oracle.ManagedDataAccess.Client;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// Реализация моста между КПП и ИТС.
    /// </summary>
    public class BridgeImpl : 
        InitializableImpl, 
        IConnectionHolderCallback<OracleConnection>, 
        IConnectionHolderCallback<OPCServer>
    {
        private static readonly ILog logger = LogManager.GetLogger("BridgeImpl");
        private static readonly TimeSpan checkInterval = TimeSpan.FromSeconds(20);

        private IBuffer dataBuffer;
        private IKpp kppConnector;
        private IIts itsConnector;

        private OracleConnectionHolder oracleConnectionHolder;
        private OpcConnectionHolder opcConnectionHolder;

        private LinesConfiguration configuration;

        /// <summary>
        /// Таймер для запуска проверок.
        /// </summary>
        private Timer checkTimer;

        /// <summary>
        /// Вызывает асинхронну проверку времени и передачу данных.
        /// </summary>
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Минута последней проверки.
        /// </summary>
        private int lastMinutes = -1;

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");

            var appPath = Application.StartupPath + "/";
            configuration = LinesConfiguration.LoadFromFile(appPath + "Settings/LinesConfiguration.xml");

            var network = ConnectionConfiguration.Default;

            oracleConnectionHolder = new OracleConnectionHolder(network.OracleConnectionString);
            oracleConnectionHolder.SetCheckConnectionInterval(TimeSpan.FromMinutes(1));
            oracleConnectionHolder.SetReconnectionInterval(TimeSpan.FromMinutes(1));
            oracleConnectionHolder.SetHolderName("Oracle");
            oracleConnectionHolder.Subscribe(this);
            
            opcConnectionHolder = new OpcConnectionHolder(network.OpcServerName, network.OpcServerHost);
            opcConnectionHolder.SetCheckConnectionInterval(TimeSpan.FromMinutes(1));
            opcConnectionHolder.SetReconnectionInterval(TimeSpan.FromMinutes(1));
            opcConnectionHolder.SetHolderName("OPC");
            opcConnectionHolder.Subscribe(this);
            
            dataBuffer = new MemoryBufferImpl();

            kppConnector = new KppImpl();
            kppConnector.SetConnectionHolder(opcConnectionHolder);
            var parameters = new List<QueryParameter>();
            foreach (var lineConfiguration in configuration.Lines) {
                parameters.AddRange(lineConfiguration.Parameters);
            }
            kppConnector.SetParametersNames(parameters.ToArray());

            itsConnector = new ItsImpl();
            itsConnector.SetConnectionHoder(oracleConnectionHolder);
            itsConnector.SetLinesConfiguration(configuration);
            itsConnector.Initialize();

            // Открытие соединений.
            opcConnectionHolder.TryConnect();
            oracleConnectionHolder.TryConnect();
            opcConnectionHolder.Start();
            oracleConnectionHolder.Start();

            checkTimer = new Timer(checkInterval.TotalMilliseconds);
            checkTimer.Elapsed += CheckerTimerTick;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += DoCheckLines;

            logger.Info("Инициализация завершена.");
        }        

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            try {
                kppConnector.Uninitialize();
                itsConnector.Uninitialize();

                opcConnectionHolder.Stop();
                oracleConnectionHolder.Stop();

                opcConnectionHolder.Dispose();
                oracleConnectionHolder.Dispose();
            }
            catch (Exception ex) {
                logger.Error("Ошибка при деинициализации: " + ex.Message);
            }

            logger.Info("Деинициализация завершена.");
        }

        public void OnConnected(IConnectionHolder<OracleConnection> aConnectionHolder)
        {
            logger.Info("Подключен " + aConnectionHolder.GetHolderName());
            TryStoreBufferedValues();
        }        

        public void OnDisconnected(IConnectionHolder<OracleConnection> aConnectionHolder)
        {
            logger.Info("Отключен " + aConnectionHolder.GetHolderName());
        }

        public void OnConnected(IConnectionHolder<OPCServer> aConnectionHolder)
        {
            logger.Info("Подключен " + aConnectionHolder.GetHolderName());
            try {
                if (!kppConnector.IsInitialized()) {
                    kppConnector.Initialize();
                }
                checkTimer.Start();
            }
            catch (Exception ex) {
                logger.Error("Ошибка при инициализации kppConnector: " + ex.Message);
            }
        }

        public void OnDisconnected(IConnectionHolder<OPCServer> aConnectionHolder)
        {
            logger.Info("Отключен " + aConnectionHolder.GetHolderName());
            checkTimer.Stop();
        }        

        private void CheckerTimerTick(object sender, ElapsedEventArgs e)
        {
            if (!backgroundWorker.IsBusy) {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void DoCheckLines(object sender, DoWorkEventArgs e)
        {            
            var currentTime = DateTime.Now;            
            var currentMinute = currentTime.Hour*60 + currentTime.Minute;
            if (currentMinute == lastMinutes) {
                return;
            }

            lastMinutes = currentMinute;
            foreach (var lineConfiguration in configuration.Lines) {
                if (currentMinute%lineConfiguration.Interval == 0) {
                    logger.Info("Получение данных серии " + lineConfiguration.Id);
                    ValuesPackage package;
                    var isReadyParameters = TryGetValuesPackages(lineConfiguration, out package);                    
                    if (isReadyParameters) {
                        package.Time = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                                                currentTime.Hour, currentTime.Minute, 0);
                        logger.Info(string.Format("Передача данных серии {0} в ИТС.", lineConfiguration.Id));
                        var isStored = itsConnector.TryWritePackage(package);
                        if (!isStored) {      
                            logger.Info(string.Format("Сохранение данных серии {0} " +
                                                      "в буфер", lineConfiguration.Id));
                            dataBuffer.AddValues(package);
                        }
                    }
                }
            }
        }

        private bool TryGetValuesPackages(LineConfiguration aLineConfiguration, out ValuesPackage aPackage)
        {
            aPackage = new ValuesPackage();
            aPackage.LineId = aLineConfiguration.Id;                        
            try {
                var parameters = new List<ValueParameter>();
                foreach (var queryParameter in aLineConfiguration.Parameters) {
                    var parameterValue = kppConnector.GetValue(queryParameter);
                    parameters.Add(parameterValue);
                }

                aPackage.Parameters = parameters.ToArray();
            }
            catch (Exception ex) {
                logger.Error("Ошибка при получении данных: " + ex.Message);
                aPackage = null;
                return false;
            }

            return true;
        }

        private bool TryStoreBufferedValues()
        {
            if (dataBuffer.IsEmpty()) {
                return true;
            }

            var packages = dataBuffer.GetValues();
            try {
                logger.Info("Отправка данных из буфера в ИТС. Данных для сохранения: " + packages.Length);
                if (itsConnector.TryWritePackages(packages)) {
                    logger.Info("Очистка буфера.");
                    dataBuffer.Clear();
                }

                return true;
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении беферизированных данных.");
                return false;
            }
        }
    }
}
