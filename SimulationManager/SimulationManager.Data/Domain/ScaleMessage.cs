using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Data.Domain
{
    public class ScaleMessage : ISerializable
    {
        public bool IsReset
        {
            get;
            set;
        }

        public int InstanceCount
        {
            get;
            set;
        }

        public string MessageId
        {
            get;
            set;
        }

        public string PopReceipt
        {
            get;
            set;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
