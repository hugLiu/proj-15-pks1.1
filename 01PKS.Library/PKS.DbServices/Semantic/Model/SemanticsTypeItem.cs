using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    [Serializable]
    public class SemanticsTypeItem
    {
        public string SR { get; set; }

        public string CCCode1 { get; set; }

        public string CCCode2 { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }
    }
}
