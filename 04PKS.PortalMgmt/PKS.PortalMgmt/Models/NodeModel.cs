using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models
{
    [Serializable]
    public class NodeModel
    {
        public string id { get; set; }
        public string text { get; set; }

        public string pid { get; set; }

        public bool isLeaf { get; set; }

        public bool asyncLoad { get; set; }

    }
}