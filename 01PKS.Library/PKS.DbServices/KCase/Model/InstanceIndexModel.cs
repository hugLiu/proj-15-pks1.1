using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class InstanceIndexModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Bo { get; set; }
        public String Remark { get; set; }
        public String Author { get; set; }
        public String Auditor { get; set; }
        public String KCaseTheme { get; set; }
        public String KCaseCategory { get; set; }
        public byte[] Chart { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
