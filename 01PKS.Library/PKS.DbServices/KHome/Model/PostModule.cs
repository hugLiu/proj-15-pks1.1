using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KHome.Model
{
    public class PostModule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }
    }
}
