using System.Data;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Jurassic.AppCenter;
using Jurassic.AppCenter.Models;
using Jurassic.Com.OfficeLib;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.EFProvider;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Jurassic.CommonModels;
using Jurassic.Com.DB;
using Jurassic.WebFrame.Models;
using Jurassic.WebTemplate.ViewModel;
using Jurassic.WebQuery.Models;
using Jurassic.WebQuery;
using Jurassic.WebSchedule;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 此控制器演示了一般性的从业务逻辑(M)到控制器(C)再到页面(V)的处理逻辑
    /// </summary>
    public class HelloWorldController : BaseController
    {
        private HelloWorldModelService _service;
        //
        // GET: /HelloWorld/

        /// <summary>
        /// 使用依赖注入自动注入服务参数，此处构造函数的参数类型
        /// 没有用接口而用的实际的类，则不需在global.asax文件中写注入逻辑
        /// </summary>
        /// <param name="service"></param>
        public HelloWorldController(HelloWorldModelService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.QueryType = typeof(HelloWorldModel);
            return View();
        }

        public JsonResult GetData(string query)
        {
            if (query.IsEmpty())
            {
                return Json(_service.GetData(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var queryManager = SiteManager.Get<AdvQueryManager>();
                var qr = queryManager.Query(_service.GetData().AsQueryable(), query);
                return Json(qr, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(HelloWorldModel model)
        {
            if (_service.GetData().Count >= 8)
            {
                return JsonTips("error", "商品数量太多了！");
            }
            model.SalerName = CurrentUser.Name;
            _service.Add(model);
            return JsonTips("success", "商品添加成功", model.Name + "添加成功！");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _service.GetData().First(m => m.Id == id);

            _service.Remove(model);

            return JsonTips("success", "商品删除成功", model.Name + "删除成功！");
        }

        public ActionResult ReportViewer()
        {
            ViewData["DataSet"] = _service.GetData();
            return View();
        }

        public ActionResult DemoIndex()
        {
            return View();
        }

        public ActionResult HtmlEditor()
        {
            return View(new EditorModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HtmlEditor(EditorModel editorModel)
        {
            return JsonTips("success", "成功提交了Text", editorModel);
        }

        public ActionResult RegUser()
        {
            return View();
        }

        public ActionResult TestTreeGrid()
        {
            return View();
        }

        public int ValidUserName(string name)
        {
            //检查用户是否已存在
            if (AppManager.Instance.UserManager.GetAll().Where(u => u.Name == name).ToList().Count > 0)
            {
                return 1;
            }
            return 0;
        }

        [HttpPost]
        public ActionResult RegUser(RegModel user)
        {
            //验证数据合法性
            if (!ModelState.IsValid)
            {
                return JsonTips();
            }

            //检查用户是否已存在
            if (AppManager.Instance.UserManager.GetAll().Where(u => u.Name == user.UserName).ToList().Count > 0)
            {
                return null;
            }

            //保存新用户信息
            AppUser newUser = new UserConfig
            {
                Email = user.Email,
                PhoneNumber = user.Phone,
                Name = user.UserName,
                Password = user.Password,
            };

            //获取所有的角色权限
            List<string> roleIds = new List<string>();
            AppManager.Instance.RoleManager.GetAll().Each(r => roleIds.Add(r.Id.ToString()));
            newUser.RoleIds = roleIds;

            AppManager.Instance.UserManager.Add(newUser);

            return JsonTipsLang("success", null, "注册成功", new { Url = Url.Action("Index", "Home") });
        }

        /// <summary>
        /// 不用框架布局页的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMyLayout()
        {
            return View();
        }
    }

    public class EditorModel
    {
        public string Author { get; set; }
        public string Title { get; set; }

        public string Text { get; set; }
    }

    public class HelloWorldModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal Price { get; set; }

        public string SalerName { get; set; }
    }

    /// <summary>
    /// 模拟一个业务服务
    /// </summary>
    public class HelloWorldModelService
    {
        private static List<HelloWorldModel> _demoList =
    new List<HelloWorldModel>
            {
                new HelloWorldModel{ Id=1, Name="IPhone6", Price=4999,CreateTime=DateTime.Now.AddDays(-3.2)},
                new HelloWorldModel{ Id=2, Name="Huawei", Price=3988,CreateTime=DateTime.Now.AddDays(-10.7)},
                new HelloWorldModel{ Id=3, Name="中兴", Price=2000,CreateTime=DateTime.Now.AddDays(-1.5)},
                new HelloWorldModel{ Id=4, Name="Samsung", Price=2600,CreateTime=DateTime.Now.AddDays(-6.8)},
            };

        public List<HelloWorldModel> GetData()
        {
            return _demoList;
        }

        public void Add(HelloWorldModel model)
        {
            model.CreateTime = DateTime.Now;
            model.Id = _demoList.Max(m => m.Id) + 1;
            _demoList.Add(model);
        }

        public void Remove(HelloWorldModel model)
        {
            //模拟底层出错的情况
            if (_demoList.Count < 2)
            {
                throw new JException("必须得留一个");
            }
            _demoList.Remove(model);
        }

        public DataTable GetExcelPreView(string fileName)
        {
            ExcelHelper excelHelper = new ExcelHelper(fileName);
            DataTable tb = excelHelper.ExcelToDataTable("myTable", false);
            return tb;
        }

    }
}
