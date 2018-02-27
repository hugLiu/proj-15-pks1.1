using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Schedule
{
    /// <summary>
    /// 返回当前用户菜单信息附加项的接口
    /// </summary>
    public interface IMenuExtInfoService
    {
        IEnumerable<MenuExtInfo> GetMenuExtInfos(int userId);
    }
}
