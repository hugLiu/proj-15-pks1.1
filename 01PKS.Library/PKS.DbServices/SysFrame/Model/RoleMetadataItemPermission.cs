using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.SysFrame.Model
{
    public class RoleMetadataItemPermission
    {
        public int MetadataItemId { get; set; }

      //  public int RoleMetaId { get; set; }

        public string MetadataItemName { get; set; }

        public bool IsAble { get; set; }



        public int MetadataPermissionId { get; set; }

        public int RoleId { get; set; }

        public int MetadataId { get; set; }

        public bool IsValid { get; set; }

        public string MetaDataType { get;set; }

        public string MetadataName { get; set; }
    }
}
