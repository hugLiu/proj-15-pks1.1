using PKS.SZXT.Core.DTO;
using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ProjectDynamic
{
    public interface IOperationProject : IViewService, IUserBehaviorAnalysis
    {
        /// <summary>获取项目运行头条</summary>
        object GetProjectHeadlines(int topCount);
        /// <summary>获取项目运行进度图数据</summary>
        object GetProjectProgress();
        /// <summary>获取项目进展情况表数据</summary>
        object GetProjectProgress2();
        /// <summary>获取项目管理数据</summary>
        object GetProjectManagement();
       
    }
}
