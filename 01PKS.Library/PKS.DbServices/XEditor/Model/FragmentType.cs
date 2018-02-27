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
    public class FragmentType
    {
        public int Id { get; set; }

        public string Code { get; set; }

     
        public string Name { get; set; }

   
        public string VueTag { get; set; }

        public bool HasTextTemplate { get; set; }

        public int? Category { get; set; }

        public int OrderNumber { get; set; }

        public string ImageUrl { get; set; }
        public List<FragmentTypeParam> ComParams { get; set; }
    }
}
