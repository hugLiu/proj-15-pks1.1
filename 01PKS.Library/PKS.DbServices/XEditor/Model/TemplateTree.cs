using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.DbServices.XEditor.Model
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class TemplateTree
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public bool IsDefault { get; set; }
        public int TemplateCategoryId { get; set; }
        public int SubSystemId { get; set; }
        public int TemplateUrlId { get; set; }
        public string InstanceClass { get; set; }

        public bool IsTemplate { get; set; }

        public string NodeId { get; set; }
        public string ParentNodeId { get; set; }
    }
}
