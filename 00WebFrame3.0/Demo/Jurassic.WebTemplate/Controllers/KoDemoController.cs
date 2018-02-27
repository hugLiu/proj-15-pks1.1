using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    public class KoDemoController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 请求用户数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUsers()
        {
            return Json(
                new List<UserDemoMode>
                {
                    new UserDemoMode
                    {
                        Id = 1,
                        Name = "张三",
                        Age = 20,
                        Sex = "男"
                    },
                    new UserDemoMode
                    {
                         Id = 2,
                        Name = "李四",
                        Age = 25,
                        Sex = "男"
                    },
                    new UserDemoMode
                    {
                         Id = 3,
                        Name = "小小",
                        Age = 21,
                        Sex = "女"
                    },
                    new UserDemoMode
                    {
                         Id = 4,
                        Name = "Sam",
                        Age = 30,
                        Sex = "男"
                    },
                }
                ,JsonRequestBehavior.AllowGet);
        }

    }

    public class UserDemoMode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
    }
}
