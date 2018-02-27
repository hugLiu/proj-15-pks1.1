using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.PageEngine.Param
{
    /// <summary>
    /// 文本模板参数[替换文本变量]
    /// </summary>
    public class TextTemplateParam : VParam
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
    }
}
