using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SimulationManager.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SimulationService : ISimulationService
    {
        public string RunExperiment(string projectId, string repeatCount, string experimentId)
        {
            return "Not Implemented Yet!";
        }

        public Experiment GetWork(string workerId, string status)
        {
            return new Experiment();
        }

        public void LoadupQueue(string msgCount)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.FromConfigurationSetting("queueconnectionstring");

            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("poolqueue");

            queue.AddMessage(new CloudQueueMessage("sample" + DateTime.UtcNow.Ticks), TimeSpan.FromMinutes(10));
        }
    }
}
