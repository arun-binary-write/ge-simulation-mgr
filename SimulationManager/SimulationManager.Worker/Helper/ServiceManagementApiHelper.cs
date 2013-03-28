using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SimulationManager.Worker.Helper;

namespace SimulationManager.Worker
{
    public static class ServiceManagementApiHelper
    {
        public static HttpWebRequest GetRequestObject(Uri uri, string httpWebMethod)
        {
            X509Certificate2 x509Certificate2 = new X509Certificate2("geautoscale.pfx", "!ge123");

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.Method = httpWebMethod;
            httpWebRequest.Headers.Add("x-ms-version", "2012-08-01");
            httpWebRequest.ClientCertificates.Add(x509Certificate2);
            httpWebRequest.ContentType = "application/xml";

            return httpWebRequest;
        }

        public static string GetOperationStatus(string requestId, string subscriptionId)
        {
            String responseFromServer = string.Empty;
            String uriString = String.Format("https://management.core.windows.net/{1}/operations/{0}", requestId, subscriptionId);
            Uri uri = new Uri(uriString);
            HttpWebRequest httpWebRequest = GetRequestObject(uri, "GET");
            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseFromServer = reader.ReadToEnd();
                }
            }
            return responseFromServer;
        }

        public static Deployment GetDeployment(string serviceName, string subscriptionId)
        {
            Deployment azureServiceDeployment = new Deployment();
            String uriString = String.Format("https://management.core.windows.net/{1}/services/hostedservices/{0}/deploymentslots/production", serviceName, subscriptionId);
            Uri uri = new Uri(uriString);
            HttpWebRequest httpWebRequest = GetRequestObject(uri, "GET");
            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();

                using (StreamReader reader = new StreamReader(dataStream))
                {
                    using (XmlReader xmlReader = XmlReader.Create(dataStream))
                    {

                        azureServiceDeployment.ReadXml(xmlReader);
                    }
                }
            }
            return azureServiceDeployment;
        }

        public static string SetRoleInstanceCount(int count, bool increase,string serviceName,string subscriptionId)
        {
            var deployment = ServiceManagementApiHelper.GetDeployment(serviceName, subscriptionId);

            int existingInstanceCount=0;
            XNamespace ns = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration";
            var configuration = XDocument.Parse(ServiceManagementApiHelper.DecodeFromBase64String(deployment.Configuration));
            var role = configuration.Root.Elements(ns + "Role")
                            .Where(p => string.Compare(p.Attribute("name").Value, "", true) == 0)
                            .SingleOrDefault();

            if (role != null)
            {
                //role.Element(ns + "Instances").SetAttributeValue("count", this.Count);
                Int32.TryParse(role.Element(ns + "Instances").Attribute("count").Value, out existingInstanceCount);
            }

            if (existingInstanceCount != 0)
            {
                existingInstanceCount += 2;
                role.Element(ns + "Instances").SetAttributeValue("count", existingInstanceCount);
            }

            var updateConfiguration = ServiceManagementApiHelper.EncodeToBase64String(configuration.ToString());

            String requestId = String.Empty;
            String uriString = string.Format("https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/production/?comp=config", subscriptionId, serviceName);
            Uri uri = new Uri(uriString);
            HttpWebRequest httpWebRequest = GetRequestObject(uri, "POST");

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                byte[] bytes = new byte[updateConfiguration.Length * sizeof(char)];
                System.Buffer.BlockCopy(updateConfiguration.ToCharArray(), 0, bytes, 0, bytes.Length);
                requestStream.Write(bytes, 0, bytes.Count());
                
            }

            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                requestId = response.Headers["x-ms-request-id"];
            }
            return requestId;
        }

        public static string DecodeFromBase64String(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(original));
        }

        public static string EncodeToBase64String(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
        }
    }
}
