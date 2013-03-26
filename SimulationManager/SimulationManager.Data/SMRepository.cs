using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SimulationManager.Data.Domain;

namespace SimulationManager.Data
{
    public class SMRepository
    {
        public void AddExperiment(int experimentId, int projectId, int noofreps, string connectionString, int replication)
        {
            using (gesimcontrolEntities1 entities = new gesimcontrolEntities1())
            {
                Experiment experiment = new Experiment()
                {
                    ExpID = experimentId,
                    ProjectID = projectId,
                    NumOfRuns = noofreps
                };

                entities.Experiments.Add(experiment);

                Project project = new Project()
                {
                    ProjectID = projectId,
                    ConnectionString = connectionString
                };

                entities.Projects.Add(project);

                DateTime now = DateTime.UtcNow;
                long simulationId = DateTime.UtcNow.Ticks;
               Int64 runid= Convert.ToInt64(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}", now.Year, now.DayOfYear, now.Hour, now.Minute, now.Second));

                for (int repsCount = 1; repsCount <= noofreps; repsCount++)
                {
                    Simulation simulation = new Simulation()
                    {
                        SimulationId = simulationId.ToString(),
                        Repitition = repsCount,
                        RunID = runid,
                        projectId = project.ProjectID
                    };
                    entities.Simulations.Add(simulation);
                }


                entities.SaveChanges();
            }


        }

        public ExperimentInfo GetWork(string workerid, string status)
        {
            ExperimentInfo experimentInfo = null;

            switch (status.ToLower())
            {
                case "completed":
                    {
                        SetComplete(workerid);
                        experimentInfo = GetNewWork(workerid, status);
                    }
                    break;
                case "failed":
                    {
                        SetFailed(workerid);
                    }
                    break;
                default:
                    {
                        experimentInfo = GetNewWork(workerid, status);
                    }
                    break;
            }

          
            return experimentInfo;

        }

        private ExperimentInfo GetNewWork(string workerid, string status)
        {
            ExperimentInfo experimentInfo = null;

            using (gesimcontrolEntities1 entities = new gesimcontrolEntities1())
            {
                var simulationInfo = (from simulation in entities.Simulations
                                      where string.IsNullOrEmpty(simulation.Status) == true
                                      orderby simulation.SimulationId, simulation.RunID
                                      select simulation).FirstOrDefault();

                if (simulationInfo != null)
                {
                    simulationInfo.WorkerID = workerid;
                    simulationInfo.Status = status;

                    var existingworker = (from worker in entities.Workers
                                          where worker.WorkerId == workerid
                                          select worker).FirstOrDefault();

                    if (existingworker != null)
                    {
                        existingworker.SimulationID=simulationInfo.SimulationId;
                        existingworker.Status = status;
                    }
                    else
                    {
                        Worker newWorker = new Worker()
                        {
                            WorkerId = workerid,
                            SimulationID = simulationInfo.SimulationId,
                            Status = status
                        };

                        entities.Workers.Add(newWorker);
                    }
                    
                    entities.SaveChanges();

                    var projectInfo = (from project in entities.Projects
                                       where project.ProjectID == simulationInfo.projectId
                                       select project).FirstOrDefault();

                    var experimentobj = (from experiment in entities.Experiments
                                         where experiment.ProjectID == projectInfo.ProjectID
                                         select experiment).FirstOrDefault();


                    experimentInfo = new ExperimentInfo()
                    {
                        ProjectId = projectInfo.ProjectID.ToString(),
                        ConnectionString = projectInfo.ConnectionString,
                        ExperimentId = experimentobj.ExpID.ToString(),
                        RepetitionNo = simulationInfo.Repitition.ToString()
                    };
                }

            
            }
            return experimentInfo;
        }

        private void SetComplete(string workerid)
        {
            using(gesimcontrolEntities1 entities=new gesimcontrolEntities1())
            {
                var workerInfo=(from worker in entities.Workers
                               where worker.WorkerId==workerid
                                    select worker).First();

                var simulationInfo = (from simulation in entities.Simulations
                                      where simulation.WorkerID == workerid
                                      select simulation).First();
                simulationInfo.Status = "Completed";
                workerInfo.Status = "Completed";
                
                entities.SaveChanges();
            }
        }

        private void SetFailed(string workerid)
        {
            using (gesimcontrolEntities1 entities = new gesimcontrolEntities1())
            {
                var workerInfo = (from worker in entities.Workers
                                  where worker.WorkerId == workerid 
                                  select worker).First();

                var simulationInfo = (from simulation in entities.Simulations
                                      where simulation.SimulationId==workerInfo.SimulationID
                                      select simulation).ToList();

                simulationInfo.ForEach(item => 
                {
                    item.Status = "Failed";
                });
                
                workerInfo.Status = "Failed";

                entities.SaveChanges();
            }
        }
    }
}
