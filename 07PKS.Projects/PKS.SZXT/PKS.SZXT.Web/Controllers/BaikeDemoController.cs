using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.DbServices;
using PKS.DbServices.KManage;
using PKS.DbServices.KManage.Model;
using PKS.PageEngine;
using PKS.PageEngine.Extensions;
using PKS.PageEngine.Query;
using PKS.SZXT.Web.Common;

namespace PKS.SZXT.Web.Controllers
{
    [AllowAnonymous]
    public class BaikeDemoController : Controller
    {
        public ActionResult Index()
        {
            string boName = Convert.ToString(Request.Params["boname"]);
            if (string.IsNullOrWhiteSpace(boName))
                boName = "well-02";
          
            PageContext pageContext=new PageContext();
            pageContext.AddContextParam("boname",boName);
            pageContext.AddContextParam("well", boName);
            pageContext.AddContextParam("botype", BoType.Well);
            pageContext.AddContextParam("instanceclass", GetBoInstanceClassName(BoType.Well));

            var queryParamDemo = JsonConvert.SerializeObject(GetQueryPlanDemo());
            var pageRenderEngine=new BaikePageEngine(pageContext);
            pageRenderEngine.Load();
            return View(pageRenderEngine);
        }

        private  string GetBoInstanceClassName(BoType botype)
        {
            if (botype == BoType.Well)
                return "井";
            if (botype == BoType.Trap)
                return "圈闭";
            if (botype == BoType.Basin)
                return "盆地";
            if (botype == BoType.FirstStructure)
                return "一级构造";
            if (botype == BoType.SecondStructure)
                return "二级构造";
            return string.Empty;
        }

        public QueryPlan GetQueryPlanDemo()
        {
            QueryPlan plan = new QueryPlan();
            List<QueryField> queryTags = new List<QueryField>();
            queryTags.Add(new QueryField
            {
                OperationType = OperationType.Equal,
                FieldName = "pt",
                FieldQueryType = FieldQueryType.Fixed,
                Value = "测试"
            });
            queryTags.Add(new QueryField
            {
                OperationType = OperationType.Equal,
                FieldName = "bowell",
                FieldQueryType = FieldQueryType.Variable,
                Value = "@well"
            });
            plan.Fields = queryTags;
            plan.TopCount = 10;
            List<QueryOrder> queryOrders = new List<QueryOrder>();
            queryOrders.Add(new QueryOrder
            {
                OrderDirection = "asc",
                FieldName = "iiid"
            });
            plan.QueryOrders = queryOrders;
            return plan;

        }
    }
}