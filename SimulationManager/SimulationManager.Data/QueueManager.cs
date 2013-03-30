using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using SimulationManager.Data.Domain;

namespace SimulationManager.Data
{
    public class QueueManager
    {
        CloudStorageAccount cloudStorageAccount;
        CloudQueueClient queueClient;
        CloudQueue queue;

        public QueueManager()
        {
            string queueConnectionString = RoleEnvironment.GetConfigurationSettingValue("queueconnectionstring");
            string queueName = RoleEnvironment.GetConfigurationSettingValue("queuename");
            cloudStorageAccount = CloudStorageAccount.Parse(queueConnectionString);
            queueClient = cloudStorageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(queueName);
        }

        public void AddMessage(ScaleMessage scaleMessage)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScaleMessage));
            byte[] bytes; 

            using (MemoryStream memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, scaleMessage);
                bytes = new byte[memStream.Length];
                memStream.Seek(0, SeekOrigin.Begin);
                memStream.Read(bytes, 0, bytes.Count());
            }

            queue.AddMessage(new CloudQueueMessage(bytes));
        }

        public ScaleMessage GetMessage()
        {
            ScaleMessage scaleMessage = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ScaleMessage));
            
            var message=queue.GetMessage(TimeSpan.FromMinutes(45));
            if (message != null)
            {
                using (MemoryStream memStream = new MemoryStream(message.AsBytes))
                {
                    memStream.Seek(0, SeekOrigin.Begin);
                    scaleMessage = (ScaleMessage)serializer.Deserialize(memStream);
                }

                scaleMessage.PopReceipt = message.PopReceipt;
                scaleMessage.MessageId = message.Id;
            }           
          
            return scaleMessage;            
        }

        public void DeleteMessage(ScaleMessage message)
        {
            queue.DeleteMessage(message.MessageId, message.PopReceipt);
        }

        public void CreateQueue()
        {
            queue.CreateIfNotExist();
        }
    }
}
