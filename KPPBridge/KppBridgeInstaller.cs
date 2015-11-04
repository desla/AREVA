using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Alvasoft.KPPBridge
{
    [RunInstaller(true)]
    public partial class KppBridgeInstaller : System.Configuration.Install.Installer
    {
        public KppBridgeInstaller()
        {
            InitializeComponent();
        }
    }
}
