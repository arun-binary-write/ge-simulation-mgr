using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulationManager.Data.Domain
{
    public class ExperimentInfo

    {   public string ProjectId
        {
            get;
            set;
        }
        public string ExperimentId
        {
            get;
            set;
        }
        
        public string RepetitionNo
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }
    }
}
