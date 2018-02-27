using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    interface ISchemaScriptProvider
    {
        /// <summary>
        /// 获取数据库架构脚本字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetSchemaScriptDictionary();

        string GetCurrentFrameSchemaVersion();
    }
}
