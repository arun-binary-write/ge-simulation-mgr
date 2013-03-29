using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [DataContract(Name = "ChangeConfiguration", Namespace = Constants.ServiceManagementNS)]
    public class ChangeConfigurationInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Configuration { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
