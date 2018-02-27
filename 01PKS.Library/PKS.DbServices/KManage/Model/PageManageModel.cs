using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage.Model
{
    public class PageManageModel
    {
        public int SubSystemId { get; set; }
        public int UrlId { get; set; }
        public int DefaultTempId { get; set; }
        public string GroupName { get; set; }
        public string RObject { get; set; }
        public string InstanceClass { get; set; }
        public List<ComboItem> Params { get; set; }
    }
}
