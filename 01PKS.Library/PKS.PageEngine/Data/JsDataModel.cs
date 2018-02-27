using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PKS.PageEngine.Data
{
    /// <summary>
    /// 用于放在界面jsModel变量,键为m_加组件Id
    /// </summary>

    public class JsDataModel:Dictionary<string,Dictionary<string,object>>
    {
    }
}