using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbModels.PortalMgmt
{
    public class PKS_ROLE_MAP
    {
        public int Id { get; set; }
        public string OrgName { get; set; }
        public string OriginalId { get; set; }
        public string OriginalPId { get; set; }
        public int RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public virtual WEBPAGES_ROLES Role {get;set;}
    }
}
