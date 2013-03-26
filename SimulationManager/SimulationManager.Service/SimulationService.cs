using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using SimulationManager.Data;

namespace SimulationManager.Service
{
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SimulationService : ISimulationService
    {
       public bool RunExperiment(string experimentid,string projectId, string noofreps, string connectionstring, string replication)
        {            
            try
            {             
                SMRepository repository = new SMRepository();
                repository.AddExperiment(Int16.Parse(experimentid), Int16.Parse(projectId), Int16.Parse(noofreps), connectionstring, Int16.Parse(replication));
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}-{1}",DateTime.UtcNow,ex.ToString());
                return false;
            }
        }


       public Experiment GetWork(string workerid, string status)
       {
           SMRepository repository = new SMRepository();
           Experiment experiment = null;

           var result = repository.GetWork(workerid, status);

           if (result != null)
           {
                experiment = new Experiment()
               {
                   ConnectionString = result.ConnectionString,
                   ExperimentId = result.ExperimentId,
                   ProjectId = result.ProjectId,
                   RepetitionNo = result.RepetitionNo
               };
           }
           return experiment;
       }
    }
}