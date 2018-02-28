using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models
{
    public class TemplateParameterDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public int OrderNumber { get; set; }
        public bool IsParameter { get; set; }
        public int? ParentId { get; set; }
    }
}