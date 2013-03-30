using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
namespace SimulationManager.Data
{
    public class BlobManager
    {
        CloudStorageAccount cloudStorageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;
        CloudBlob cloudBlob;

        public BlobManager()
        {
            string queueConnectionString = RoleEnvironment.GetConfigurationSettingValue("queueconnectionstring");
            var taskmanagerPath = RoleEnvironment.GetConfigurationSettingValue("distributor").Split('\\');
            string container=taskmanagerPath[0];
            string blob=taskmanagerPath[1];
            cloudStorageAccount = CloudStorageAccount.Parse(queueConnectionString);
            blobClient = cloudStorageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(container);
            cloudBlob = blobContainer.GetBlobReference(blob);
        }

        public void CreateBlob()
        {
            blobContainer.CreateIfNotExist();
            
            cloudBlob.FetchAttributes();
        }
    }
}
