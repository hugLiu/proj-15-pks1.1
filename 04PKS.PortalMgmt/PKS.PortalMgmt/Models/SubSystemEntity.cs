using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models
{
    [Serializable]
    public class SubSystemEntity
    {
         public int _id { get; set; }
        public int _uid { get; set; }
        public string _state { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RootUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}