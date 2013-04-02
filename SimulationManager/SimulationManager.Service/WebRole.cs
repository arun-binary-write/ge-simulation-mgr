using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SimulationManager.Service
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // To enable the AzureLocalStorageTraceListner, uncomment relevent section in the web.config           

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(2);
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Information;

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);         


            return base.OnStart();
        }
    }
}
