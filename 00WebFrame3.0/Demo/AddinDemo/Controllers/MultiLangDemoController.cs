using AddinDemo.Models;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddinDemo.Controllers
{
    /// <summary>
    /// 多语言组件的使用示例
    /// </summary>
    public class MultiLangDemoController : BaseController
    {
        ModelDataService<StockModel, Stock> _dataService;

        /// <summary>
        /// 多语言组件需要使用ModelDataSerivce,如果不使用ModelDataSerive,在数据保存时可以考虑调用LangDataMapper.SaveLanguages()
        /// 来手动保存表单传递的语言数据， 在数据查询时，可以参考LangDataMapper中的方法来对多语言字段时行手工映射。
        /// </summary>
        /// <param name="dataService"></param>
        public MultiLangDemoController(ModelDataService<StockModel, Stock> dataService)
        {
            _dataService = dataService;
        }
        //
        // GET: /MultiLangDemo/

        public ActionResult Index()
        {
            ViewBag.QueryType = typeof(StockModel); //高级查询对于多语言字段仍然适用
            return View();
        }

        [AdvQuery]
        public JsonResult GetData()
        {
            return Json(_dataService.GetQuery(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int id = 0)
        {
            StockModel model = _dataService.GetByKey(id) ?? new StockModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(StockModel model)
        {
            if (model.Id == 0)
            {
                _dataService.Add(model);
                return JsonTips("success", "添加成功！");
            }
            else
            {
                _dataService.Change(model);
                return JsonTips("success", "修改成功！");
            }
        }
    }
}
