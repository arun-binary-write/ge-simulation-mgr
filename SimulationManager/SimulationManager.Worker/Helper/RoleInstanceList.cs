using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [CollectionDataContract(Name = "RoleInstanceList", ItemName = "RoleInstance", Namespace = Constants.ServiceManagementNS)]
    public class RoleInstanceList : List<RoleInstance>
    {
        public RoleInstanceList()
        {
        }

        public RoleInstanceList(IEnumerable<RoleInstance> roles)
            : base(roles)
        {
        }
    }
}
