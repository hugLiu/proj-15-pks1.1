using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbModels.PortalMgmt
{
    [Table("PKS_ROLE_METADATAITEMPERMISSION")]
    public class PKS_ROLE_METADATAITEMPERMISSION
    {
        public int Id { get; set; }

        //[JsonIgnore]
        public int RoleMetaId { get; set; }

        public string MetadataItemName { get; set; }

        public bool IsAble { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [JsonIgnore]
        public virtual PKS_ROLE_METADATAPERMISSION RoleMeta { get; set; }

    }

}
