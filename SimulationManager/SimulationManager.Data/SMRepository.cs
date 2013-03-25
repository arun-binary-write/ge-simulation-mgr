using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulationManager.Data
{
    public class SMRepository
    {
        public void AddExperiment(int experimentId,int projectId,int noofreps,string connectionString,int replication)
        {            
            using(gesimcontrolEntities1 entities=new gesimcontrolEntities1() )
            {
                Experiment experiment = new Experiment()
                {
                    ExpID=experimentId,
                    ProjectID=projectId,
                    NumOfRuns=noofreps
                };

                entities.Experiments.Add(experiment);

                Project project = new Project()
                {
                    ProjectID=projectId,
                    ConnectionString=connectionString
                };

                entities.Projects.Add(project);

                long simulationId = DateTime.UtcNow.Ticks;

                for (int repsCount = 1; repsCount <= noofreps; repsCount++)
                {
                    Simulation simulation = new Simulation()
                    {
                        SimulationId=simulationId.ToString(),
                        Repitition=repsCount
                    };
                    entities.Simulations.Add(simulation);
                }

                entities.SaveChanges();
            }

            
        }
    }
}
