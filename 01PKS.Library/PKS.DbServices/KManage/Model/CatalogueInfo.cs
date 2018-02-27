using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PKS.DbModels;
using PKS.Utils;

namespace PKS.DbServices.KManage.Model
{
    /// <summary>
    /// 目录信息
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class CatalogueInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int LevelNumber { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentId { get; set; }
        public int TemplateId { get; set; }

        public string NodeId { get; set; }
        public string ParentNodeId { get; set; }

        public string PlaceHolderId { get; set; }

        [JsonIgnore]
        public PKS_KTEMPLATE_CATALOGUE Catalogue { get; set; }
    }
}
