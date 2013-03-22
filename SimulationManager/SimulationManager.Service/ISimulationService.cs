using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace SimulationManager.Service
{
    public interface ISimulationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{projectId}/{repeatCount}/{experimentId}")]
        string RunExperiment(string projectId, string repeatCount, string experimentId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{workerId}/{status}")]
        Experiment GetWork(string workerId, string status);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{msgCount}")]
        void LoadupQueue(string msgCount);
    }
}