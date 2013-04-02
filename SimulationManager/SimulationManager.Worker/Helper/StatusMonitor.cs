using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using SimulationManager.Data;

namespace SimulationManager.Worker.Helper
{
    public class StatusMonitor
    {
        public void Run()
        {
            gesimcontrolEntities entities = new gesimcontrolEntities();

            Stopwatch stopWatch = new Stopwatch();

            bool isAllDone = false;

            while (true)
            {
                Thread.Sleep(TimeSpan.FromHours(1));
                stopWatch.Restart();
                do
                {
                    var totalexecutions = entities.Simulations.Count();
                    var totalCompleted = entities.Simulations.Where(item => item.Status.ToLower() == "completed").Count();
                    var totalFailed = entities.Simulations.Where(item => item.Status.ToLower() == "failed").Count();

                    if (totalexecutions == (totalCompleted + totalFailed))
                    {
                        isAllDone = true;
                    }
                    else
                    {
                        isAllDone = false;
                        break;
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(5));
                } while (stopWatch.Elapsed.TotalMinutes < 30);

                string subscriptionId = RoleEnvironment.GetConfigurationSettingValue("eesubscriptionid");
                string service = RoleEnvironment.GetConfigurationSettingValue("eeservicename");
                string role = RoleEnvironment.GetConfigurationSettingValue("eerolename");

                if (isAllDone)
                {
                    Trace.TraceInformation("{0}-Begin reset role instance count to 2.}",DateTime.UtcNow);
                    ScalingApi.SetRoleInstanceCount(subscriptionId, service, role, 2, true);
                }
                
            }
        }
    }
}
