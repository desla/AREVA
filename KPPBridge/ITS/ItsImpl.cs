namespace Alvasoft.KPPBridge.ITS
{
    using System;
    using Common;
    using Configuration;
    using ConnectionHolders;
    using log4net;
    using Oracle.ManagedDataAccess.Client;
    using Utils.Activity;

    /// <summary>
    /// Реализация взаимодействия с ИТС.
    /// </summary>
    public class ItsImpl : 
        InitializableImpl, 
        IIts
    {
        private static readonly ILog logger = LogManager.GetLogger("ItsImpl");

        private OracleConnectionHolder oracleConnection;
        private LinesConfiguration configuration;

        private const string DATA_TIME  = "data_time";
        private const string LINE_ID    = "line_id";

        public void SetConnectionHoder(OracleConnectionHolder aConnectionHolder)
        {
            oracleConnection = aConnectionHolder;
        }

        public void SetLinesConfiguration(LinesConfiguration aConfiguration)
        {
            configuration = aConfiguration;
        }

        public bool TryWritePackage(ValuesPackage aPackage)
        {
            if (aPackage == null) {
                logger.Info("Параметр aPackage равен null.");
                return false;
            }

            return TryWritePackages(new [] {aPackage});
        }

        public bool TryWritePackages(ValuesPackage[] aPackages)
        {
            if (aPackages == null) {
                logger.Info("Параметр aPackages равен null.");
                return false;
            }
            
            try {
                var connection = oracleConnection.WaitConnection();
                using (var transaction = connection.BeginTransaction())
                using (var command = new OracleCommand(configuration.InsertQuery)) {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.Parameters.Add(DATA_TIME, OracleDbType.Date);
                    command.Parameters.Add(LINE_ID, OracleDbType.Int32);
                    foreach (var parameter in configuration.Parameters) {
                        command.Parameters.Add(parameter, OracleDbType.Double);
                    }

                    foreach (var valuesPackage in aPackages) {
                        command.Parameters[DATA_TIME].Value = valuesPackage.Time;
                        command.Parameters[LINE_ID].Value = valuesPackage.LineId;
                        foreach (var parametrValue in valuesPackage.Parameters) {
                            command.Parameters[parametrValue.Name].Value = parametrValue.Value;
                        }

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex) {
                logger.Error("Ошибка при сохранении данных в ИТС: " + ex.Message);
                return false;
            }
            finally {
                oracleConnection.ReleaseConnection();
            }
        }
    }
}
