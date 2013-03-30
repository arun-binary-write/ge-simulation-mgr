using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SimulationManager.Worker.Helper;

namespace SimulationManager.Worker
{
    public static class ServiceManagementApiHelper
    {
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
