using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using SimulationManager.Data;

namespace SimulationManager.Service
{
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SimulationService : ISimulationService
    {
       public bool RunExperiment(string runid, string projectId, string noofreps, string connectionstring, string replication)
        {
            try
            {
                SMRepository repository = new SMRepository();
                repository.AddExperiment(Int16.Parse(runid), Int16.Parse(projectId), Int16.Parse(noofreps), connectionstring, Int16.Parse(replication));
                return true;
            }
            catch (Exception ex)
            {
                // TODO : pack the exception in WebContext and return to client.
                return false;
            }
        }


       public Experiment GetWork(string workerid, string status)
       {
           return new Experiment(); // TODO : Implementation
       }
    }
}