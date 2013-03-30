using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimulationManager.Data;

namespace SimulationManager.Worker.Helper
{
    public class StatusMonitor
    {
        public void Run()
        {
            gesimcontrolEntities entities = new gesimcontrolEntities();           
            
            var totalexecutions=entities.Simulations.Count();

            var totalCompleted = entities.Simulations.Where(item => item.Status == "Completed").Count();
            var totalFailed = entities.Simulations.Where(item => item.Status == "Failed").Count();
            
            
        }
    }
}
