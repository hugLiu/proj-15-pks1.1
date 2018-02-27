using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#pragma warning disable 1591

namespace PKS.Core.UEditor
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContextBase context) : base(context) { }

        public override void Process()
        {
            WriteJson(Config.Items);
        }
    }
}