﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18408
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alvasoft.KPPBridge.Configuration {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class ConnectionConfiguration : global::System.Configuration.ApplicationSettingsBase {
        
        private static ConnectionConfiguration defaultInstance = ((ConnectionConfiguration)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ConnectionConfiguration())));
        
        public static ConnectionConfiguration Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=192.168.1.11;User Id=areva;Password=areva;")]
        public string OracleConnectionString {
            get {
                return ((string)(this["OracleConnectionString"]));
            }
            set {
                this["OracleConnectionString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.11")]
        public string OpcServerHost {
            get {
                return ((string)(this["OpcServerHost"]));
            }
            set {
                this["OpcServerHost"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Kepware.KEPServerEX.V5")]
        public string OpcServerName {
            get {
                return ((string)(this["OpcServerName"]));
            }
            set {
                this["OpcServerName"] = value;
            }
        }
    }
}
