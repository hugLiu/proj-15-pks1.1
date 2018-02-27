using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.DbServices.KManage.Model
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class FragmentTypeParam
    {
        public int Id { get; set; }

        public int FragmentTypeId { get; set; }

  
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 对应Es标签【用来定义Es输出参数】
        /// </summary>
        public string Metadata { get; set; }

        public string DefaultValue { get; set; }
        //public bool ISDEFAULT { get; set; }

        public string DataType { get; set; }

        public string Value { get; set; }

    }
}
