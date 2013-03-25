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
        [DataMember]
        public string ProjectId
        {
            get;
            set;
        }

        [DataMember]
        public string ExperimentId
        {
            get;
            set;
        }

        [DataMember]
        public string RepetitionNo
        {
            get;
            set;
        }

        [DataMember]
        public string ConnectionString
        {
            get;
            set;
        }
    }
}