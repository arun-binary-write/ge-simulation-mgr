using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using SimulationManager.Data;
using SimulationManager.Worker.Helper;

namespace SimulationManager.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        QueueManager queueManager;
        string subscriptionId;
        string serviceName;
        string roleName;

        public override void Run()
        {
            if (RoleEnvironment.CurrentRoleInstance.Id.EndsWith("0"))
            {
                var statusMonitorTask=Task.Factory.StartNew(() => 
                {
                    StatusMonitor statusMonitor = new StatusMonitor();
                    statusMonitor.Run();
                });
                
            }

            while (true)
            {

                try
                {
                    var message = queueManager.GetMessage();
                    if (message != null)
                    {
                        var trackingId = ScalingApi.SetRoleInstanceCount(subscriptionId, serviceName, roleName, message.InstanceCount, message.IsReset);
                        queueManager.DeleteMessage(message);
                        if (!string.IsNullOrEmpty(trackingId))
                        {
                            Operation operationStatus;
                            do
                            {
                                Thread.Sleep(10000);
                                operationStatus = ScalingApi.GetOperationStatus("788d90bb-b1d5-44eb-a335-aa8569d69bc6", trackingId);
                            }
                            while (operationStatus.Status == "InProgress");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            string conString = RoleEnvironment.GetConfigurationSettingValue("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
            RoleInstanceDiagnosticManager diagmgr = new RoleInstanceDiagnosticManager(CloudStorageAccount.Parse(conString), RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);

            var config = diagmgr.GetCurrentConfiguration();
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(2);

            diagmgr.SetCurrentConfiguration(config);

            queueManager = new QueueManager();
            queueManager.CreateQueue();

            subscriptionId = RoleEnvironment.GetConfigurationSettingValue("eesubscriptionid");
            serviceName = RoleEnvironment.GetConfigurationSettingValue("eeservicename");
            roleName = RoleEnvironment.GetConfigurationSettingValue("eerolename");

            return base.OnStart();
        }
    }
}
