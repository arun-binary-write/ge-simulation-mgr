using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class RoleInstance
    {
        [DataMember(Order = 1)]
        public string RoleName { get; set; }

        [DataMember(Order = 2)]
        public string InstanceName { get; set; }

        [DataMember(Order = 3)]
        public string InstanceStatus { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
