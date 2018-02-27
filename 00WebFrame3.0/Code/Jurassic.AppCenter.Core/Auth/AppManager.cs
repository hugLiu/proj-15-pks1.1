using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Jurassic.Com.Tools;
using System.Web;
using System.Web.Security;
using System.Collections;
using Jurassic.AppCenter;
using Jurassic.AppCenter.Models;
using Jurassic.AppCenter.AppServices;
using Jurassic.AppCenter.Caches;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 应用程序总控类，提供用户认证和权限判定的基本方法
    /// </summary>
    public class AppManager
    {
        DataManager<AppUser> mUserMgr;
        DataManager<AppRole> mRoleMgr;
        DataManager<AppFunction> mFunctionMgr;
        IStateProvider mStateProvider;
        static readonly string SetupFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "App_Data", "SetupModel.json");

        /// <summary>
        /// 用户管理类
        /// </summary>
        public DataManager<AppUser> UserManager
        {
            get { return mUserMgr; }
        }

        /// <summary>
        /// 角色管理类
        /// </summary>
        public DataManager<AppRole> RoleManager
        {
            get { return mRoleMgr; }
        }

        /// <summary>
        /// 功能管理类
        /// </summary>
        public DataManager<AppFunction> FunctionManager
        {
            get { return mFunctionMgr; }
        }

        /// <summary>
        /// 登录方法的提供程序
        /// </summary>
        public IStateProvider StateProvider
        {
            get { return mStateProvider; }
            set { mStateProvider = value; }
        }

        IStateManager mStateManager;
        /// <summary>
        /// 登录状态管理接口实现
        /// </summary>
        public IStateManager StateManager
        {
            get { return mStateManager; }
            set { mStateManager = value; }
        }

        private ITypeProvider mTypeProvider;
        /// <summary>
        /// 用于WCF服务时，识别AppUser,AppRole,AppFunction等对象的子对象的类型标识器
        /// </summary>
        public ITypeProvider KnownTypeProvider
        {
            get { return mTypeProvider; }
            set { mTypeProvider = value; }
        }

        //public void RefreshData()
        //{
        //    mUserMgr.Reset();
        //    mRoleMgr.Reset();
        //    mFunctionMgr.Reset();
        //}

        /// <summary>
        /// 判断对象是否需要初始化
        /// </summary>
        public bool NeedInit
        {
            get
            {
                //bool needInit = FunctionManager.DataList.Count == 0
                //    || RoleManager.DataList.Count == 0
                //    || UserManager.DataList.Count == 0;
                //if (!needInit)
                //{
                var needInit = !File.Exists(SetupFileName);
                //}
                return needInit;
            }
        }

        /// <summary>
        /// 重新开始初始化
        /// </summary>
        public void StartReInit()
        {
            if (System.IO.File.Exists(SetupFileName))
            {
                System.IO.File.Delete(SetupFileName);
            }
        }

        static AppManager mInstance;
        /// <summary>
        /// AppManager的单例
        /// </summary>
        public static AppManager Instance
        {
            get
            {
                if (mInstance == null) mInstance = new AppManager();
                return mInstance;
            }
        }

        //static AppManagerRole mInstanceRole;
        /// <summary>
        /// AppManager的单例
        /// </summary>
        //public static AppManager InstanceRole
        //{
        //    get
        //    {
        //        if (mInstance == null) mInstance = new AppManagerRole();
        //        return mInstance;
        //    }
        //}


        /// <summary>
        /// 功能数据提供方（该提供方一般使用自带的）
        /// </summary>
        public IDataProvider<AppFunction> FunctionProvider
        {
            get
            {
                return FunctionManager.Provider;
            }
            set
            {
                FunctionManager.Provider = value;
                if (FunctionManager.Provider.GetType() != typeof(LocalDataProvider<AppFunction>))
                {
                    RoleProvider = mRoleProvider;
                }
            }
        }

        private IDataProvider<AppRole> mRoleProvider;
        /// <summary>
        /// 角色数据提供方
        /// </summary>
        public IDataProvider<AppRole> RoleProvider
        {
            get
            {
                return mRoleProvider;
            }
            set
            {
                mRoleProvider = value;
                if (value != null)
                {
                    //当使用缺省的LocalDataProvider<AppFunction>作功能数据提供方时，要多一层包装存
                    // FunctionId和role的关系。当使用其他数据提供方时，不使用
                    if (FunctionProvider.GetType() == typeof(LocalDataProvider<AppFunction>))
                    {
                        var cachedData = new CachedList<AppRole>();
                        RoleManager.Provider = new RoleDataWrapper(mRoleProvider, cachedData);
                        mRoleMgr.AfterSaved = (t) =>
                        {
                            cachedData.Save();
                        };
                    }
                    else
                    {
                        RoleManager.Provider = mRoleProvider;
                    }
                }
            }
        }

        /// <summary>
        /// 用户数据提供方
        /// </summary>
        public IDataProvider<AppUser> UserProvider
        {
            get
            {
                return UserManager.Provider;
            }
            set
            {
                UserManager.Provider = value;
            }
        }

        /// <summary>
        /// 默认的权限控制类型
        /// </summary>
        public JAuthType DefaultAuthType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserName()
        {
            string userName = null;
            if (HttpContext.Current == null)
            {
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            return userName;
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserId()
        {
            string name = GetCurrentUserName();
            if (name == null) return null;
            var appUser = UserManager.GetByName(name);
            if (appUser == null) return null;
            return appUser.Id;
        }

        /// <summary>
        /// 私有构造函数，防止用户随便New出来。
        /// </summary>
        private AppManager()
        {
            mUserMgr = new DataManager<AppUser>();
            mRoleMgr = new DataManager<AppRole>();
            mFunctionMgr = new DataManager<AppFunction>();
            mStateManager = new StateManager(false);
            //提供缺省的数据提供者
            mFunctionMgr.Provider = new LocalDataProvider<AppFunction>();
            RoleProvider = new RoleDataProvider();
            mUserMgr.Provider = new UserDataProvider();
        }

        /// <summary>
        /// 判断某用户是否有某功能ID的访问权限，这一般是表示用户已经确定点击了某个功能
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="functionId">功能ID</param>
        /// <returns>有或无</returns>
        internal virtual bool HasRightId(string userName, string functionId)
        {
            if (HasRightIdCore(userName, functionId))
            {
                AppUser user = UserManager.GetByName(userName);
                user.LastOpTime = TimeHelper.ServerTime;
                user.IsOnline = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证该用户是否拥有该模块功能权限
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="functionId">模块id</param>
        /// <returns>true=是 false=否</returns>
        private bool HasRightIdCore(string userName, string functionId)
        {
            AppUser user = UserManager.GetByName(userName);
            AppFunction func = FunctionManager.GetById(functionId);
            /*
             * 返回值在原有基础上添加了用户与模块功能的判断 by_zjf
             *       (user.FunctionIds.Count() == 0  || user.FunctionIds.Contains(functionId))
             * 判断规则优先顺序
             *      1.已设置用户与模块功能权限,验证是否存在该模块功能.
             *      2.未设置用户与模块功能权限,根据模块关联角色的关系进行验证.    
             */
            return user != null
                    && (func == null
                    || func.AuthType == JAuthType.AllUsers
                    || func.AuthType == JAuthType.EveryOne
                    || user.FunctionIds.Contains(functionId)
                    || user.RoleIds.Any(rId => RoleManager.GetById(rId).FunctionIds.Contains(functionId))
                )
                && (user.IsDefaultRole
                || func.AuthType == JAuthType.AllUsers
                || func.AuthType == JAuthType.EveryOne
                || user.FunctionIds.Contains(functionId)
                    );
        }

        /// <summary>
        /// 判断用户是否有某个资源的访问权限
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="location">资源定位标识</param>
        /// <returns>是/否</returns>
        public virtual bool HasRight(string userName, string location, WebMethod method = WebMethod.GET)
        {
            AppFunction function = FunctionManager.GetAll()
                .FirstOrDefault(f => f.MatchLocation(location)
                    && f.Method == method);
            //    if (function == null) return false; //在没初始化功能集合时，只要用户登录了就能访问所有功能
            return function == null || HasRightIdCore(userName, function.Id);
        }

        /// <summary>
        /// 获取URL对应的FunctionId
        /// </summary>
        /// <param name="location">地址</param>
        /// <param name="method">提交方法</param>
        /// <returns>FunctionId</returns>
        public virtual string GetFunctionId(string location, WebMethod method = WebMethod.GET)
        {
            AppFunction function = FunctionManager.GetAll()
                .FirstOrDefault(f => f.MatchLocation(location)
                    && f.Method == method);
            return function == null ? "" : function.Id;
        }

        /// <summary>
        /// 获取URL对应的Function
        /// </summary>
        /// <param name="location">地址</param>
        /// <param name="method">提交方法</param>
        /// <returns>Function实体</returns>
        public virtual AppFunction GetFunctionByLocation(string location, WebMethod method = WebMethod.GET)
        {
            AppFunction function = FunctionManager.GetAll()
                .FirstOrDefault(f => f.MatchLocation(location)
                    && f.Method == method);
            return function;
        }

        /// <summary>
        /// 登录的公共方法
        /// </summary>
        /// <param name="LoginModel">登录数据实体，包括用户名、密码、记住我等信息</param>
        /// <returns>登录状态的枚举</returns>
        /// <exception cref="Jurassic.AppCenter.JException">JException</exception>
        public LoginState Login(LoginModel model)
        {
            if (StateProvider == null)
            {
                throw new JException("没有提供StateProvider，请提供实现了IStateProvider接口的登录提供程序。");
            }

            var result = StateProvider.Login(model);
            if (result == LoginState.OK)
            {
                AppUser user = UserManager.GetByName(model.UserName);


                if (user == null)
                {
                    if (UserProvider.GetType() == typeof(UserDataProvider))
                    {
                        user = new AppUser() { Name = model.UserName };
                        UserManager.Add(user);
                    }
                    else //当使用外部提供程序时，找不到用户则重新读取全部用户数据
                    {
                        UserManager.Clear();
                        user = UserManager.GetByName(model.UserName);
                    }
                    if (user == null)
                    {
                        throw new JException("User Not Exists:" + model.UserName);
                    }
                }
                user.LastOpTime = TimeHelper.ServerTime;
                user.IsOnline = true;
                if (HttpContext.Current != null)
                {
                    if (!StateProvider.SetAuthCookie(model))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 退出登录的公共方法
        /// </summary>
        /// <param name="userName">要退出登录的用户名</param>
        public void Logout(string userName)
        {
            if (StateProvider == null)
            {
                throw new ArgumentNullException("没有提供StateProvider，请提供实现了IStateProvider接口的登录提供程序。");
            }

            if (!userName.IsEmpty())
            {
                StateProvider.Logout(userName);
                FormsAuthentication.SignOut();
                AppUser user = UserManager.GetByName(userName);
                if (user != null)
                {
                    user.IsOnline = false;
                }
            }
        }

        /// <summary>
        /// 获取所有登录用户
        /// </summary>
        /// <returns>所有登录用户</returns>
        public List<AppUser> GetOnlineUsers()
        {
            var onLineUsers = UserManager.GetAll()
                .Where(u => u.IsOnline)
                .OrderByDescending(u => u.LastOpTime)
                .ToList();

            return onLineUsers;
        }

        /// <summary>
        /// 获取当前用户有权限的功能ID列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>功能ID列表</returns>
        public IEnumerable<string> GetRightIds(string userName)
        {
            var user = UserManager.GetByName(userName);

            IEnumerable<string> roleIds = new string[0];
            if (user != null)
            {
                if (!user.IsDefaultRole)
                {
                    return user.FunctionIds;
                }

                foreach (var roleId in user.RoleIds)
                {
                    roleIds = roleIds.Union(RoleManager.GetById(roleId).FunctionIds);
                }
            }

            return roleIds;
        }

        /// <summary>
        /// 获取当前用户有权限的功能列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>功能ID列表</returns>
        internal IEnumerable<AppFunction> GetHasRightFunctions(string userName)
        {
            var roleIds = GetRightIds(userName);
            return AppManager.Instance.FunctionManager.GetAll().Where(f => roleIds.Contains(f.Id));
        }

        /// <summary>
        /// 获取当前用户无权限的功能ID列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>功能ID列表</returns>
        internal IEnumerable<string> GetForbiddenIds(string userName)
        {
            return FunctionManager.GetAll()
                .Where(func => func.AuthType == JAuthType.NeedAuth)
                .Select(func => func.Id)
                .Except(GetRightIds(userName))
               .Union(FunctionManager.GetAll()
                    .Where(func => func.AuthType == JAuthType.Forbidden)
                    .Select(func => func.Id));
        }

        /// <summary>
        /// 获取当前用户无权限的功能列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IEnumerable<AppFunction> GetForbiddenFunctions(string userName)
        {
            var roleIds = GetForbiddenIds(userName);
            return AppManager.Instance.FunctionManager.GetAll().Where(f => roleIds.Contains(f.Id));
        }

        /// <summary>
        /// 获取当前用户可操作的菜单
        /// </summary>
        /// <returns></returns>
        public List<AppFunction> GetUserMenus(string userName)
        {
            var userMenus = mFunctionMgr.GetAll()
                .Where(func => (func.Visible & VisibleType.Menu) == VisibleType.Menu && HasRightId(userName, func.Id))
                .OrderBy(func => func.Ord)
                .ThenBy(func => func.Id).ToList();
            return userMenus;
        }

        //private bool TreeVisible(string userName, AppFunction func)
        //{
        //    while (func != null)
        //    {
        //        if ((func.Visible & VisibleType.Menu) == 0) return false;
        //        if (!HasRightId(userName, func.Id)) return false;
        //        func = mFunctionMgr.GetById(func.ParentId);
        //    }
        //    return true;
        //}

        /// <summary>
        /// 获取所有权限管理的控制菜单
        /// </summary>
        /// <returns></returns>
        public List<AppFunction> GetAllRoleMenus()
        {
            return mFunctionMgr.GetAll()
                .Where(d => TreeNeedAuth(d))
                .OrderBy(d => d.Ord)
                .ThenBy(d => d.Id).ToList();
        }

        private bool TreeNeedAuth(AppFunction func)
        {
            if ((func.Visible & VisibleType.IsService) == VisibleType.IsService) return false;
            if (func.AuthType == JAuthType.NeedAuth) return true;
            foreach (var child in mFunctionMgr.GetAll().Where(c => c.ParentId == func.Id))
            {
                if (TreeNeedAuth(child))
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 结束初始化时向磁盘写入一个标识文件
        /// </summary>
        /// <param name="setupModel">要写入文件的可序列化对象</param>
        public void EndInit(object setupModel)
        {
            System.IO.File.WriteAllText(AppManager.SetupFileName, JsonHelper.ToJson(setupModel));
        }

        /// <summary>
        /// 为WCF服务提供AppUser、AppRole、AppFunction等的已知派生类型
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAppTypes(ICustomAttributeProvider provider)
        {
            if (Instance.KnownTypeProvider == null)
            {
                return new List<Type>();
            }
            return Instance.KnownTypeProvider.GetTypes();
        }

        /// <summary>
        /// 获取所有部件列表
        /// </summary>
        /// <returns>部件列表</returns>
        public IEnumerable<AppFunction> GetWidgetFunctions()
        {
            return AppManager.Instance.FunctionManager.GetAll()
               .Where(f => (f.Visible & VisibleType.Widget) == VisibleType.Widget)
               .OrderBy(f => f.Ord)
               .ThenBy(f => f.Id)
               .ToArray();
        }
    }
}