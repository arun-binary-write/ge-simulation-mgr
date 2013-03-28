using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.Autoscaling;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;


namespace SimulationManager.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
      //  RoleInstancesManager roleInstancesManager=null;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("SimulationManager.Worker entry point called", "Information");

            while (true)
            {
                int existingInstanceCount = 0;


               var trackindgId= ServiceManagementApiHelper.SetRoleInstanceCount(2, true, "geexpmgr", "788d90bb-b1d5-44eb-a335-aa8569d69bc6");

                Trace.WriteLine("Working", "Information");
                Thread.Sleep(10000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //roleInstancesManager = new RoleInstancesManager();

            var autoscaler = EnterpriseLibraryContainer.Current.GetInstance<Autoscaler>();
            autoscaler.Start();
            return base.OnStart();
        }
    }
}
