using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage.Model
{
    public class KInstance
    {
        public int? Id { get; set; }
        public int KTemplateId { get; set; }
        public string Instance { get; set; }
        public string InstanceClass { get; set; }
        public string StaticUrl { get; set; }
        public DateTime? StaticDate { get; set; }
    }
}
