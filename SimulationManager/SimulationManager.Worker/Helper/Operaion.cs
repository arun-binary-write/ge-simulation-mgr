using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class Operation : IExtensibleDataObject
    {
        [DataMember(Name = "ID", Order = 1)]
        public string OperationTrackingId { get; set; }

        [DataMember(Order = 2)]
        public string Status { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public int HttpStatusCode { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public ServiceManagementError Error { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
