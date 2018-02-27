using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using System.IO;
using Jurassic.AppCenter.Models;
using Jurassic.AppCenter;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// AppCenter的初始化向导
    /// </summary>
    [JSetup]
    [JAuth(JAuthType.Ignore)]
    public class SetupController : BaseController
    {
        //
        // GET: /AppCenter/Setup/

        /// <summary>
        /// 初始化向导的主页,起始页嵌在主页之内
        /// </summary>
        /// <returns>初始化向导的主页</returns>
        public ActionResult Index()
        {
            return View();
        }

        // [JMenu("起始页")]
        /// <summary>
        /// 初始化向导起始页，开始程序设置
        /// </summary>
        /// <returns>初始化向导起始页</returns>
        public ActionResult Start()
        {
            return View();
        }

        /// <summary>
        /// 返回查找功能列表的视图
        /// </summary>
        /// <returns>查找功能列表的视图</returns>
        public ActionResult GetFunctions()
        {
            ViewBag.DllPaths = String.Join(Environment.NewLine, GetBinPaths());
            return View();
        }

        /// <summary>
        /// 根据DLL列表查找功能
        /// </summary>
        /// <param name="dllPaths">DLL路径名，以换行分隔</param>
        /// <returns>重定向到SetAdmin</returns>
        [HttpPost]
        public ActionResult GetFunctions(string dllPaths)
        {
            var arrPaths = dllPaths.Replace("\r\n", "\n")
                            .Split('\n')
                            .Select(s => s.Trim())
                            .Where(s => !s.IsEmpty());
            IEnumerable<string> dlls = new List<string>();
            foreach (string path in arrPaths)
            {
                dlls = dlls.Union(SearchDll(path));
            }
            new FunctionInitializer().InitFunctions(dlls);

            SetTips("success", "Scan_Finish", "Scan_Finished_And_Got_All_Functions");
            return RedirectToAction("SetAdmin");
        }

        /// <summary>
        /// 返回设置管理员角色和用户的表单
        /// </summary>
        /// <returns>设置管理员角色和用户的表单</returns>
        public ActionResult SetAdmin()
        {
            ViewBag.RoleNames = String.Join(", ", AppManager.Instance.RoleManager.GetAll().Select(role => role.Name));
            ViewBag.UserNames = String.Join(", ", AppManager.Instance.UserManager.GetAll().Take(10).Select(user => user.Name));
            if (AppManager.Instance.UserManager.Count > 10) ViewBag.UserNames += ", ...";
            return View(new SetupModel());
        }

        /// <summary>
        /// 接受管理员角色和用户表单的输入，并设置管理员的角色和账户
        /// </summary>
        /// <param name="setupModel">包含管理员角色和账户名的数据对象</param>
        /// <returns>完成页</returns>
        [HttpPost]
        public ActionResult SetAdmin(SetupModel setupModel)
        {
            var roleMgr = AppManager.Instance.RoleManager;
            var userMgr = AppManager.Instance.UserManager;

            var role = roleMgr.GetByName(setupModel.AdminRoleName);
            string msg = "";
            if (role == null)
            {
                role = roleMgr.Create();
                role.Name = setupModel.AdminRoleName;
                roleMgr.Add(role);
                msg = "Role_Created_{0}";
            }
            else
            {
                msg = "Role_Exists_{0}";
            }
            //授予该角色所有权限
            role.FunctionIds = AppManager.Instance.FunctionManager.GetAll().Select(d => d.Id).ToList();

            var user = userMgr.GetByName(setupModel.AdminUserName);

            if (user == null)
            {
                user = userMgr.Create();
                user.Name = setupModel.AdminUserName;
                user.AddRoleId(role.Id);
                user.Password = "123456";
                userMgr.Add(user);
                msg += "+User_Created_{1}"; //带上加号方便由多语言组件以此为标记切出关键词
            }
            else
            {
                user.AddRoleId(role.Id);
                userMgr.Change(user);
                msg += "+User_Exists_{1}";
            }
            SetTips("success", null, msg, null, user.Name, role.Name);
            return RedirectToAction("Finish");
        }

        public ActionResult Finish()
        {
            AppManager.Instance.EndInit("App Initialized Finished, Delete this file to ReInit.");
            return View("SetupFinish");
        }

        string[] SearchDll(string dllPath)
        {
            DirectoryInfo binDir = new DirectoryInfo(dllPath);
            if (binDir.Exists)
            {
                return binDir.GetFiles("*.DLL").Select(d => d.FullName).ToArray();
            }
            return new string[0];
        }

        List<string> GetBinPaths()
        {
            List<string> dllsPath = new List<string>();

            string basePath = "~/";
            DirectoryInfo baseDir = new DirectoryInfo(Server.MapPath(basePath));
            for (int i = 0; i < 10; i++)
            {
                DirectoryInfo binDir = new DirectoryInfo(Path.Combine(baseDir.FullName, "bin"));
                if (binDir.Exists)
                {
                    dllsPath.Add(binDir.FullName);
                }
                baseDir = baseDir.Parent;
                if (baseDir == null) break;
            }
            return dllsPath;
        }

    }
}
