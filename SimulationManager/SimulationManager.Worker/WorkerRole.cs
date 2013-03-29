using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
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
            while (true)
            {
                var message = queueManager.GetMessage();

                if (message != null)
                {
                    var trackingId = ScalingApi.SetRoleInstanceCount(subscriptionId,serviceName,roleName, message.InstanceCount, message.IsReset);
                    queueManager.DeleteMessage(message); 
                    Operation operationStatus;
                    do
                    {                        
                        Thread.Sleep(10000);
                        operationStatus = ScalingApi.GetOperationStatus("788d90bb-b1d5-44eb-a335-aa8569d69bc6", trackingId);                        
                    }
                    while (operationStatus.Status == "InProgress");
                    
                }
                Trace.WriteLine("Working", "Information");

                Thread.Sleep(TimeSpan.FromMinutes(5).Milliseconds);
            }
        }

        public override bool OnStart()
        {            
            ServicePointManager.DefaultConnectionLimit = 12;
            
             queueManager = new QueueManager();
            queueManager.CreateQueue();

            subscriptionId = RoleEnvironment.GetConfigurationSettingValue("eesubscriptionid");
            serviceName = RoleEnvironment.GetConfigurationSettingValue("eeservicename");
            roleName = RoleEnvironment.GetConfigurationSettingValue("eerolename");

            return base.OnStart();
        }
    }
}
