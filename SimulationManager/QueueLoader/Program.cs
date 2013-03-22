using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace QueueLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=gesimmanager;AccountKey=JbZJwq8akqXrgoyKAkwcMUYi9GKcs//QEr5HM0QXlLhAs3aknIUWr53PnAchwmrjpmQCCRq6nHEphVKf/tgY0Q==");

            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("poolqueue");

            Stopwatch stopwatch = new Stopwatch();

            while(true)
            {
                queue.AddMessage(new CloudQueueMessage("sample" + DateTime.UtcNow.Ticks), TimeSpan.FromMinutes(10));
                Thread.Sleep(1000);

                if (stopwatch.Elapsed.Minutes > 10)
                {
                    break;
                }
            }
                
        }
    }
}
