using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Jurassic.AppCenter.AppServices
{
    [ServiceContract]
    [ServiceKnownType("GetAppTypes", typeof(AppManager))]
    public interface IUserManagerService
    {
        #region 用户管理        
        
        [OperationContract]
        IEnumerable<AppUser> QueryAllUser();

        [OperationContract]
        AppUser QueryUserById(string id);

        [OperationContract]
        AppUser QueryUserByName(string name);

        [OperationContract]
        int InsertUser(AppUser user);
        
        [OperationContract]
        int UpdateUser(AppUser user);

        [OperationContract]
        int DeleteUser(string id);

        [OperationContract]
        int CheckUser(AppUser user);

        #endregion

        #region 角色管理

        [OperationContract]
        IEnumerable<AppRole> QueryAllRole();

        [OperationContract]
        AppRole QueryRoleById(string id);

        [OperationContract]
        int InsertRole(AppRole role);

        [OperationContract]
        int UpdateRole(AppRole role);

        [OperationContract]
        int DeleteRole(string id);

        #endregion

        #region 功能管理

        [OperationContract]
        IEnumerable<AppFunction> QueryAllFunction();

        [OperationContract]
        AppFunction QueryFunctionById(string id);

        [OperationContract]
        IEnumerable<AppFunction> QueryChildFunction(string id);

        [OperationContract]
        int InsertFunction(AppFunction func);

        [OperationContract]
        int UpdateFunction(AppFunction func);

        [OperationContract]
        int DeleteFunction(string id);

        #endregion

        #region 综合


        #endregion

    }
}
