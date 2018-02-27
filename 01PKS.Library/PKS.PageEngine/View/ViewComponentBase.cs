using System.Collections.Generic;
using PKS.PageEngine.Data;

namespace PKS.PageEngine.View
{

    public abstract class ViewComponentBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string,string> HtmlAttributes { get; set; }
        /// <summary>
        /// 原始编辑配置字符串【编辑工具生成的对应组件的字符串信息】
        /// </summary>
        public string OrginalEditConfig { get; set; }
        public abstract string ToHtml();

        public abstract void AddChild(ViewComponentBase childComponent);
        public abstract void RemoveChild(ViewComponentBase childComponent);
    }
}
