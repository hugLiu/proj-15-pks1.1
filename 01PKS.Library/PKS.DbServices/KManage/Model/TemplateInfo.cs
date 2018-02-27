using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage.Model
{
    public class TemplateInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TemplateCategoryId { get; set; }
        public bool IsDefault { get; set; }
        public string InstanceClass { get; set; }
        /// <summary>
        /// 编辑工具生成的字符串
        /// </summary>
        public string Template { get; set; }

    }
}
