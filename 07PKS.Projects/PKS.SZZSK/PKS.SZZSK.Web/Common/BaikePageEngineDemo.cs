using PKS.DbServices;
using PKS.DbServices.KManage.Model;
using PKS.PageEngine;
using PKS.PageEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.SZZSK.Web.Common
{
    public class BaikePageEngineDemo:BaikePageEngine
    {
        private PageContext _pageContext;
        private KManage2Service _kManage2Service;
        public BaikePageEngineDemo(PageContext pageContext) : base(pageContext)
        {
            _pageContext = pageContext;
            _kManage2Service = PKS.Core.Bootstrapper.Get<KManage2Service>();
        }

        public override TemplateInfo GetTemplate()
        {
            string boName = _pageContext.GetContextParamValue<string>("instance");
            var instanceClass = _pageContext.GetContextParamValue<string>("instanceclass");
            int urlid = _pageContext.GetContextParamValue<int>("urlid");

            var templateInfo = _kManage2Service.GetTemplateInfoWithTemplateContent(14);
            if (templateInfo == null)
                return null;
            //throw new Exception("未找到模板");
            if (string.IsNullOrWhiteSpace(templateInfo.Template))
            {
                return null;
                //throw new Exception("模板内容为空");
            }
            return templateInfo;
        }
    }
}