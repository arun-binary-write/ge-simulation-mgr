using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimulationManager.Service
{    
    [ServiceContract]
    public interface ISimulationService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/runexperiment/{experimentid}/{projectId}/{noofreps}/{connectionstring}/{replication}", ResponseFormat = WebMessageFormat.Json)]
        bool RunExperiment(string experimentid,string projectId,string noofreps,string connectionstring,string replication);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/getwork/{workerid}/{status}", ResponseFormat = WebMessageFormat.Json)]
        Experiment GetWork(string workerid, string status);
}
}
