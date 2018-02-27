using System.Collections.Generic;
using System.Text;

namespace PKS.PageEngine.View
{
    /// <summary>
    /// 容器组件
    /// </summary>
    public class ViewComponentContainer:ViewComponentBase
    {
        private List<ViewComponentBase> _children=new List<ViewComponentBase>();

        public List<ViewComponentBase> Children
        {
            get { return _children; }
        }

        public override string ToHtml()
        {
            if (_children.Count == 0)
                return OrginalEditConfig;
            StringBuilder builder = new StringBuilder("<div");
            //todo 是否设置div的宽高为100%
            if (this.HtmlAttributes != null)
            {
                foreach (var htmlAttribute in HtmlAttributes)
                {
                    builder.AppendFormat(" {0}=\"{1}\"", htmlAttribute.Key, htmlAttribute.Value);
                }
            }
            builder.Append(">");
            foreach (var child in _children)
            {
                builder.Append(child.ToHtml());
            }
            builder.Append("</div>");
            return builder.ToString();
        }

        public override void AddChild(ViewComponentBase childComponent)
        {
            _children.Add(childComponent);
        }

        public override void RemoveChild(ViewComponentBase childComponent)
        {
            _children.Remove(childComponent);
        }
    }
}
