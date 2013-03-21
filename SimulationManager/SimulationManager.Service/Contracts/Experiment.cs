using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimulationManager.Service
{
    [DataContract]
    public class Experiment
    {
        public string ProjectId
        {
            get;
            set;
        }
    }
}