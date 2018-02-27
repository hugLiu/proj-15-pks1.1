using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbModels.PortalMgmt
{
    //[Table("PKS_METADATAITEM")]
    public class PKS_METADATAITEM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public int MetadataId { get; set; }

        [Required]
        public string MetadataItemName { get; set; }

        public int? PId { get; set; }

    }

    public static partial class PermissionExtension
    {
        //public static PKS_ROLE_METADATAITEMPERMISSION ToRoleMetaPermission(this PKS_METADATAITEM sp, int roleId)
        //{
        //    var rp = new PKS_ROLE_METADATAITEMPERMISSION();
        //    rp.RoleId = roleId;
        //    rp.MetadataId = sp.MetadataId;
        //    rp.MetadataItemId = sp.Id;
        //    return rp;
        //}
    }
}
