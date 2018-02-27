using AddinDemo.Models;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddinDemo.Controllers
{
    public class PersonsController : AdvDataController<PersonModel, Person>
    {
        public PersonsController()
        {
            SiteManager.Kernel.Rebind<DbContext, ModelContext>().To<PersonContext>().WithPropertyValue("Schema", "");
        }

        public override System.Web.Mvc.ActionResult Index()
        {
            return base.Index();
        }

        protected override void BeforeShowing(PersonModel t)
        {
            t.EduHistorys = t.EduHistorys.OrderBy(e => e.StartDate).ToList();
        }

        /// <summary>
        /// 此处重写检测员工所获荣誉数量，如果一个也没有，就给一个优秀员工,
        /// 如果超过三个，则不让保存并返回错误。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected override bool BeforeSaving(PersonModel t)
        {
            if (t.Salary < 1000)
            {
                t.Salary = 1000;
            }
            if (t.Honors.Count == 0)
            {
                //如果一个也没有，则添加一个默认的
                t.Honors.Add(new HonorModel
                {
                    GetDate = DateTime.Now,
                    HonorName = "优秀员工",
                });
            }
            if (t.Honors.Count > 3)
            {
                SetTips("error", "Honor数量太多", null);
                //终止下步操作，返回错误
                return false;
            }
            return true;
        }

        public override JsonResult GetLinkedList(string linkedValue, string prop)
        {
            switch (prop)
            {
                case "City":
                    return Json(GetCitys(linkedValue), JsonRequestBehavior.AllowGet);
                default: return null;
            }
        }

        public override JsonResult GetUserDefineList(string prop)
        {
            switch (prop)
            {
                case "Country":
                    return Json(new object[] { new { id = "中国", text = "中国" }, new { id = "美国", text = "美国" }, new { text = "英国", id = "英国" } }, JsonRequestBehavior.AllowGet);
                default: return null;
            }
        }

        private object[] GetCitys(string country)
        {
            switch (country)
            {
                case "中国":
                    return new object[] { new { id = "北京", text = "北京" }, new { id = "西安", text = "西安" }, new { id = "武汉", text = "武汉" } };
                case "美国":
                    return new object[] { new { id = "华盛顿", text = "华盛顿" }, new { id = "纽约", text = "纽约" }, new { id = "费城", text = "费城" } };
                case "英国":
                    return new object[] { new { id = "伦敦", text = "伦敦" }, new { id = "曼城", text = "曼城" } };
                default:
                    return null;
            }
        }

        private object[] GetSubjects(string special)
        {
            switch (special)
            {
                case "1":
                    return new object[] { new { id = "11", text = "设计" }, new { id = "12", text = "制造" }, new { id = "13", text = "安装" } };
                case "2":
                    return new object[] { new { id = "21", text = "电视" }, new { id = "22", text = "显示器" } };
                case "3":
                    return new object[] { new { id = "31", text = "住宅" }, new { id = "32", text = "桥梁" }, new { id = "33", text = "厂房" } };
                default:
                    return new object[] { new { id = "41", text = "外科" }, new { id = "42", text = "内科" } };
            }
        }

        public override JsonResult GetDetailLinkedList(string detail, string linkedValue, string prop)
        {
            switch (prop)
            {
                case "Subject":
                    return Json(GetSubjects(linkedValue), JsonRequestBehavior.AllowGet);
                default: return null;
            }
        }
    }
}
