using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 此DEMO演示WebRepeater组件的简单用法，更复杂的用法可参见NoteBookController
    /// </summary>
    public class RepeaterController : BaseController
    {
        static RepeaterController()
        {
            Random rand = new Random(0);
            allData = Enumerable.Range(1, 1000).Select(i => new HelloWorldModel
             {
                 Id = i,
                 CreateTime = DateTime.Now.AddSeconds(rand.Next(i * 1000)),
                 Name = "Product_" + rand.Next(),
                 Price = rand.Next() / 10.2m,
                 SalerName = "Saler_" + rand.Next()
             }).ToArray().AsQueryable(); //如果先不ToArray()一下，每次结果都会不一样
        }
        //
        // GET: /Repeater/
        static IQueryable<HelloWorldModel> allData;

        public ActionResult Index(PageModel pageModel)
        {
            return View(new Pager<HelloWorldModel>(allData, pageModel.SortExpression, pageModel.PageIndex, pageModel.PageSize));
        }

        //
        // GET: /Repeater/Details/5

    }

}
