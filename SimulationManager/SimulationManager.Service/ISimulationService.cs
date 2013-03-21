using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimulationManager.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISimulationService
    {
        [OperationContract]
        [WebInvoke(Method="POST",UriTemplate="/{projectId}/{repeatCount}/{experimentId}")]
        string RunExperiment(string projectId, string repeatCount, string experimentId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{workerId}/{status}")]
        Experiment GetWork(string workerId, string status);
    }
  
}
