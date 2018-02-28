using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.DTO
{
    public class RoleMetaItemPermissionDTO
    {
        public int IndexId { get; set; }
        public int MetaId { get; set; }
        public int RoleMetaId { get; set; }

        public List<PKS_ROLE_METADATAITEMPERMISSION> WhiteList { get; set; }
        public List<PKS_ROLE_METADATAITEMPERMISSION> BlackList { get; set; }
    }
}