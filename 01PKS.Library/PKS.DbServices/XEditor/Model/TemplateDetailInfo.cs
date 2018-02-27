using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PKS.DbServices.KManage.Model;
using PKS.Utils;

namespace PKS.DbServices.XEditor.Model
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class TemplateDetailInfo
    {
        public TemplateTree TemplateInfo { get; set; }
        public List<FragmentModel> Fragments { get; set; }
        public List<CatalogueInfo> Catalogues { get; set; }
    }
}
