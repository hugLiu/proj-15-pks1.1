using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class InstanceModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String BoDescription { get; set; }
        public String Remark { get; set; }
        public String Author { get; set; }
        public String Auditor { get; set; }
        public int KCaseThemeId { get; set; }
        public String Theme { get; set; }
    }
}
