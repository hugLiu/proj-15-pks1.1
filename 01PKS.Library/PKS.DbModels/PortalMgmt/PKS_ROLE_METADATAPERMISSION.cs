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
    [Table("PKS_ROLE_METADATAPERMISSION")]
    public class PKS_ROLE_METADATAPERMISSION
    {
        public int Id { get; set; }
      
        public int RoleId { get; set; }

        public int MetadataId { get; set; }

        public bool IsValid { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        //public virtual PKS_METADATADEFINITION MetadataDefinition { get; set; }

        [JsonIgnore]
        public virtual List<PKS_ROLE_METADATAITEMPERMISSION> MetadataItemPermissioin { get; set; }

    }
}
