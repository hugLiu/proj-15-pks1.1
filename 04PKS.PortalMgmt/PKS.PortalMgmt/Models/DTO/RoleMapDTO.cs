using PKS.DbModels;
using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.DTO
{
    public class RoleMapDTO
    {
        public PKS_ROLE_MAP RoleMap { get; set; }

        public IEnumerable<WEBPAGES_ROLES> Roles { get; set; }
    }
}