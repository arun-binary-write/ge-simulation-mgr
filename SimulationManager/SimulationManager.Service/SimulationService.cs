using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationManager.Service
{
    public class SimulationService : ISimulationService
    {
        public string RunExperiment(string projectId, string repeatCount, string experimentId)
        {
            return "Not Implemented Yet!";
        }

        public Experiment GetWork(string workerId, string status)
        {
            return new Experiment();
        }
    }
}