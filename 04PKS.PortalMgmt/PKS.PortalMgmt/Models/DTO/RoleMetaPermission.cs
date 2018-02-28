using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.DTO
{
    public class RoleMetaPermissionDTO
    {
        public int Id { get; set; }
        public int? PId { get; set; }
        public int MetaId { get; set; }
        public int RoleId { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
    }

}