using System;
using System.Linq;
using System.Text;
using PKS.PageEngine.Data;
using PKS.PageEngine.EvenHandlers;

namespace PKS.PageEngine.View
{
    /// <summary>
    /// 视图组件
    /// </summary>
    public class ViewComponent:ViewComponentBase
    {
        public event EventHandler<SampleEventArgs> LoadDataEventHandler;
        public event EventHandler<EventArgs> BeforeLoadDataEventHandler;
        public event EventHandler<SampleEventArgs> AfterLoadDataEventHandler;
        /// <summary>
        /// 组件对应的片段信息
        /// </summary>
        public FragmentInfo FragmentInfo { get; set; }

        public ViewComponent(FragmentInfo fragmentInfo)
        {
            this.FragmentInfo = fragmentInfo;
           // this.Data=new object();
        }

       public object Data { get; set; }

        public override string ToHtml()
        {
            return BuildComponent();
        }

        public string FullTagName
        {
            get { return GetFullTagName(); }
        }

        private string GetFullTagName()
        {
            if(FragmentInfo==null)
                return String.Empty;
            var vueTag = string.IsNullOrWhiteSpace(FragmentInfo.FragmentVueTag) ? "text" : FragmentInfo.FragmentVueTag;
            var vuePksTag = vueTag.StartsWith("pks:") ? vueTag : ("pks:" + vueTag);
            return vuePksTag;
        }

        private string BuildComponent()
        {
            var modelName = Consts.CModelPropertyPrefix + Id;
            StringBuilder builder = new StringBuilder("<div");
            //todo 是否设置div的宽高为100%
            if (this.HtmlAttributes !=null)
            {
                foreach (var htmlAttribute in HtmlAttributes)
                {
                    builder.AppendFormat(" {0}=\"{1}\"", htmlAttribute.Key, htmlAttribute.Value);
                }
            }

            builder.Append(">");
            if (FragmentInfo != null)
            {
                //如果标记为Vue，则默认pks:text 组件
                var vuePksTag = GetFullTagName();
                builder.AppendFormat("<{0}", vuePksTag);
                if (FragmentInfo.ComponentParameters != null && FragmentInfo.ComponentParameters.Count > 0)
                {
                    foreach (var fragmentInfoComponentSysParameter in FragmentInfo.ComponentParameters)
                    {
                        //如果参数配置了对应的ES标签，则忽略当前参数
                        if(!string.IsNullOrWhiteSpace(fragmentInfoComponentSysParameter.Metadata))
                            continue;
                        builder.AppendFormat(" :{0}=\"{1}\"", fragmentInfoComponentSysParameter.Code,
                            modelName + "." + fragmentInfoComponentSysParameter.Code);
                    }
                }
                builder.Append(">");
                //如果未标记Vue标签且不存在html参数时
                //if (string.IsNullOrWhiteSpace(FragmentInfo.FragmentVueTag)&&(FragmentInfo.ComponentParameters == null||
                //    !FragmentInfo.ComponentParameters.Any(
                //        item => string.Equals(item.Code, "html", StringComparison.OrdinalIgnoreCase))))
                //{
                //    builder.AppendFormat(" :html=\"{0}\"", modelName + ".html");
                //}
                builder.AppendFormat("</{0}>", vuePksTag);

            }
            builder.Append("</div>");
            return builder.ToString();
        }

        public override void AddChild(ViewComponentBase childComponent)
        {
            
        }

        public override void RemoveChild(ViewComponentBase childComponent)
        {
           
        }

        public void BeforeLoadData()
        {
            if (BeforeLoadDataEventHandler != null)
                BeforeLoadDataEventHandler(this,new EventArgs());
        }

        public void LoadData()
        {
            if (LoadDataEventHandler != null)
            {
                var eventArgs = new SampleEventArgs() {Data = this.Data};
                LoadDataEventHandler(this, eventArgs);
                this.Data = eventArgs.Data;
            }
               
        }

        public void AfterLoadData()
        {
            if (AfterLoadDataEventHandler != null)
            {
                var eventArgs = new SampleEventArgs() { Data = this.Data };
                AfterLoadDataEventHandler(this, eventArgs);
                if (Data != eventArgs.Data)
                {
                    Data = eventArgs.Data;
                }
            }
               
        }
    }
}
