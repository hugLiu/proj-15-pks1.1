using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbModels.PortalMgmt
{
    //[Table("PKS_METADATADEFINITION")]
    public class PKS_METADATADEFINITION
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool? IsRequired { get; set; }

        public bool? IsInsideTag { get; set; }

        [StringLength(50)]
        public string DataType { get; set; }

        [StringLength(200)]
        public string DataFormat { get; set; }

        [StringLength(50)]
        public string ShowType { get; set; }

        [StringLength(50)]
        public string DataSource { get; set; }

        public bool? CanSearch { get; set; }

        public float? SearchWeight { get; set; }

        public int? OrderNumber { get; set; }

        public int? PId { get; set; }

        //public virtual IList<PKS_ROLE_METADATAPERMISSION> RoleMetadataPermission { get; set; }

    }
}
