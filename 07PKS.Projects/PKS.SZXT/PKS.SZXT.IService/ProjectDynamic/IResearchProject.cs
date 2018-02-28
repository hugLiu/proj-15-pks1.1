using PKS.SZXT.Core.DTO;
using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ProjectDynamic
{
    public interface IResearchProject : IViewService, IUserBehaviorAnalysis
    {
        /// <summary>获取项目运行头条</summary>
        object GetProjectHeadlines(int topCount);
        /// <summary>获取项目进展情况表数据</summary>
        object GetProjectProgress();
        /// <summary>获取项目详情数据_项目立项</summary>
        object GetProjectApproval(string projectName);
        /// <summary>获取项目详情数据_项目实施</summary>
        object GetProjectImplement(string projectName);
        /// <summary>获取项目详情数据_项目验收</summary>
        object GetProjectAcceptance(string projectName);


    }
}
