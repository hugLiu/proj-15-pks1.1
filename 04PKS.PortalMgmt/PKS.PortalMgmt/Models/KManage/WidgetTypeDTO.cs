using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.KManage
{
    public class WidgetTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string VueTag { get; set; }
        public bool HasTextTemplate { get; set; }
        public int OrderNumber { get; set; }
        public string ImageUrl { get; set; }
        public int? Category { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime? CREATEDDATE { get; set; }
        public string LASTUPDATEDBY { get; set; }
        public DateTime? LASTUPDATEDDATE { get; set; }
        public ICollection<WidgetTypeParamDTO> WidgetTypeParams { get; set; }
    }

    public class WidgetTypeParamDTO
    {
        public int Id { get; set; }
        public int WidgetTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string MetaData { get; set; }
        public string DefaultValue { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime? CREATEDDATE { get; set; }
        public string LASTUPDATEDBY { get; set; }
        public DateTime? LASTUPDATEDDATE { get; set; }
    }
}