namespace Alvasoft.KPPBridge.KPP
{
    using System;
    using Common;
    using Configuration;
    using ConnectionHolders;
    using log4net;
    using OpcTagAccessProvider;
    using Utils.Activity;

    /// <summary>
    /// Реализация чтения данных из КПП.
    /// </summary>
    public class KppImpl : 
        InitializableImpl, 
        IKpp
    {
        private static readonly ILog logger = LogManager.GetLogger("KppImpl");

        private OpcConnectionHolder opcConnection;
        private QueryParameter[] parameters;
        private OpcValueImpl[] opcTags;

        public void SetConnectionHolder(OpcConnectionHolder aConnectionHolder)
        {
            opcConnection = aConnectionHolder;
        }

        public void SetParametersNames(QueryParameter[] aParameters)
        {
            parameters = aParameters;
        }

        public ValueParameter GetValue(QueryParameter aParameter)
        {
            foreach (var opcTag in opcTags) {
                if (opcTag.Name == aParameter.OpcTag) {
                    var valueParameter = new ValueParameter();
                    valueParameter.Name = aParameter.Name;
                    valueParameter.Value = Convert.ToDouble(opcTag.ReadCurrentValue());
                    return valueParameter;
                }
            }

            throw new ArgumentException("Не найден ОРС тег " + aParameter.OpcTag);
        }

        protected override void DoInitialize()
        {
            logger.Info("Инициализация...");
            var server = opcConnection.WaitConnection();
            try {
                opcTags = new OpcValueImpl[parameters.Length];
                for (var i = 0; i < parameters.Length; ++i) {
                    opcTags[i] = new OpcValueImpl(server, parameters[i].OpcTag);
                    opcTags[i].Activate();
                }
            }
            finally {
                opcConnection.ReleaseConnection();
            }
            logger.Info("Инициализация завершена.");
        }

        protected override void DoUninitialize()
        {
            logger.Info("Деинициализация...");
            if (opcTags != null) {
                for (var i = 0; i < opcTags.Length; ++i) {
                    opcTags[i].Deactivate();
                }
            }
            logger.Info("Деинициалиация завершена.");
        }
    }
}
