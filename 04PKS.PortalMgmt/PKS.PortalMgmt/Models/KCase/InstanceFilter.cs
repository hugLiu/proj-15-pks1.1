using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.KCase
{
    public class InstanceFilter
    {
        public int TypeId { get; set; }
        public int ClassValueId { get; set; }
        public string VBo { get; set; }
        public string HBo { get; set; }
        public string Bo { get; set; }
        public string CaseName { get; set; }

    }
}