using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using SimulationManager.Worker.Helper;

namespace SimulationManager.Worker
{
    class ScalingApi
    {
        public static string SetRoleInstanceCount(string subscriptionId, string serviceName, string roleName, int roleCount, bool isReset)
        {
            string trackingId = null;
            HttpStatusCode? statusCode = null;
            string statusDescription = null;

            try
            {  
                var channel = CreateServiceManagementChannel("WindowsAzureEndPoint");

                var deployment = channel.GetDeploymentBySlot(subscriptionId, serviceName, "Production");

                if (string.IsNullOrEmpty(deployment.Configuration))
                    throw new WebException("Service Configuration not available");

                string configXML = ServiceManagementApiHelper.DecodeFromBase64String(deployment.Configuration);

                if (string.IsNullOrEmpty(configXML))
                    throw new WebException("Custom Exception. 800");

                XElement serviceConfig = XElement.Parse(configXML, LoadOptions.SetBaseUri);
                XNamespace xmlns = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration";

                int instanceCount = 0;

                if (isReset)
                {
                    instanceCount = 2;
                }
                else
                {
                    var currentInstanceCount = (from c in serviceConfig.Elements(xmlns + "Role")
                                                where string.Compare(c.Attribute("name").Value, roleName, true) == 0
                                                select c.Element(xmlns + "Instances").Attribute("count").Value).FirstOrDefault();

                    instanceCount = Convert.ToInt32(currentInstanceCount) + roleCount;
                }

                foreach (XElement p in serviceConfig.Elements(xmlns + "Role"))
                {
                    if (string.Compare((string)p.Attribute("name"), roleName, true) == 0)
                        p.Element(xmlns + "Instances").Attribute("count").SetValue(instanceCount.ToString());
                }

                var encodedString = ServiceManagementApiHelper.EncodeToBase64String(serviceConfig.ToString());

                using (OperationContextScope scope = new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        ChangeConfigurationInput input = new ChangeConfigurationInput { Configuration = encodedString };

                        channel.ChangeConfigurationBySlot(subscriptionId, serviceName, "production", input);

                        if (WebOperationContext.Current.IncomingResponse != null)
                        {
                            trackingId = WebOperationContext.Current.IncomingResponse.Headers[Constants.OperationTrackingIdHeader];
                            statusCode = WebOperationContext.Current.IncomingResponse.StatusCode;
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;
                            Console.WriteLine("Operation ID: {0}", trackingId);
                        }

                    }
                    catch (CommunicationException ce)
                    {
                        Trace.TraceError(ce.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            
            return trackingId;
        }

        public static Operation GetOperationStatus(string subscriptionId,string trackingId)
        {
            Operation operaionStatus = null;
            try
            {
                var channel = CreateServiceManagementChannel("WindowsAzureEndPoint");
                operaionStatus = channel.GetOperationStatus(subscriptionId, trackingId);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            return operaionStatus;
        }
        
        private static IServiceManagement CreateServiceManagementChannel(string endpointConfigurationName)
        {
            X509Certificate2 Certificate = new X509Certificate2("geautoscale.pfx", "!ge123");
            WebChannelFactory<IServiceManagement> factory = new WebChannelFactory<IServiceManagement>(endpointConfigurationName);
            factory.Endpoint.Behaviors.Add(new ClientOutputMessageInspector());
            factory.Credentials.ClientCertificate.Certificate = Certificate;
            var channel = factory.CreateChannel();
            return channel;
        }
    }
}
