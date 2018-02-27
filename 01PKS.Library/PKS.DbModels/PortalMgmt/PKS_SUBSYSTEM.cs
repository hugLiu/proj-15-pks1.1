using PKS.Models;
using System;
using System.Collections.Generic;

namespace PKS.DBModels
{
    public partial class PKS_SUBSYSTEM : IPKSSubSystemInfo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string RootUrl { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string Name { get; set; }

        public virtual IList<PKS_PERMISSION> Permissions { get; set; }
    }
}
