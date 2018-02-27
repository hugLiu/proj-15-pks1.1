using System;

namespace PKS.DBModels
{
    public class PKS_ROLE_PERMISSION
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public int IsDefault { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public virtual PKS_PERMISSION Permission { get; set; }

    }
}
