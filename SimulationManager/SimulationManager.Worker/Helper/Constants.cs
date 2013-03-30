using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    public static class Constants
    {
        public const string ServiceManagementNS = "http://schemas.microsoft.com/windowsazure";
        public const string VersionHeaderName = "x-ms-version";
        public const string OperationTrackingIdHeader = "x-ms-request-id";
        public const string VersionHeaderContent = "2009-10-01";
    }
}
