using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class UpgradeStatus : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string UpgradeType { get; set; }

        [DataMember(Order = 2)]
        public string CurrentUpgradeDomainState { get; set; }

        [DataMember(Order = 3)]
        public int CurrentUpgradeDomain { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
