using PKS.DBModels;
using System.Collections.Generic;

namespace PKS.DbModels.PortalMgmt
{
    public class PKS_PERMISSION_TYPE
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IList<PKS_PERMISSION> Permissions { get; set; }
    }
}
