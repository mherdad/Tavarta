using System.Collections.Generic;

namespace Tavarta.ServiceLayer.Security
{
    public  class PermissionRecord
    {
        public string RoleName { get; set; }
        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}
