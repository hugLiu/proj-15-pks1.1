using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;

namespace PKS.DBModels
{
    public class PKS_PERMISSION
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PermissionTypeId { get; set; }

        public string Url { get; set; }

        public int SubSystemId { get; set; }

        public int LevelNumber { get; set; }

        public int OrderNumber { get; set; }

        public int? ParentId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public virtual PKS_PERMISSION_TYPE PermissionType { get; set; }
        public virtual PKS_SUBSYSTEM SubSystem { get; set; }

        public virtual IList<PKS_ROLE_PERMISSION> RolePermissions { get; set; }

    }
}
