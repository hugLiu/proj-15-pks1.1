using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.Core.Models.ADIdentity
{
    public class AdDept
    {
        public AdDept(string name,string id,string pId)
        {
            OrgName = name;
            OriginalId = id;
            OriginalPId = pId;
        }
        public string OrgName { get; set; }
        public string OriginalId { get; set; }
        public string OriginalPId { get; set; }
    }
}
