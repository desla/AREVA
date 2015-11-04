namespace Alvasoft.KPPBridge
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using System.Windows.Forms;
    using Bridge;
    using log4net.Config;

    class KppBridgeApp : ServiceBase
    {
        private static BridgeImpl _bridge = new BridgeImpl();

        static void LoggerInitialize()
        {
            var appPath = Application.StartupPath + "/";
            XmlConfigurator.Configure(new FileInfo(appPath + "Settings/Logging.xml"));
        }

        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower().Equals("console")) {             
                LoggerInitialize();
                _bridge.Initialize();
                Console.WriteLine("Для выхода нажмите Enter...");
                Console.ReadLine();
                _bridge.Uninitialize();
            }
            else {
                ServiceBase.Run(new KppBridgeApp());
            }
        }

        protected override void OnStart(string[] args)
        {
            LoggerInitialize();
            _bridge.Initialize();
        }

        protected override void OnStop()
        {            
            _bridge.Uninitialize();
        }
    }
}
