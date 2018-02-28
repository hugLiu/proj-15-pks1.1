using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using PKS.DbServices.KManage.Model;
using PKS.PageEngine;
using PKS.PageEngine.Extensions;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;
using PKS.SZZSK.Web.Common;

namespace PKS.SZZSK.Web.Controllers
{
    //[AllowAnonymous]
    public class BaikeDemoController : Controller
    {
        public ActionResult Index()
        {
            string boName = Convert.ToString(Request.Params["boname"]);
            if (string.IsNullOrWhiteSpace(boName))
                boName = "well-02";//简单默认模板
          
            PageContext pageContext=new PageContext();
            pageContext.AddContextParam("instance",boName);
            pageContext.AddContextParam("well", boName);
            pageContext.AddContextParam("botype", BoType.Well);
            pageContext.AddContextParam("instanceclass", GetBoInstanceClassName(BoType.Well));

            var queryParamDemo = JsonConvert.SerializeObject(GetQueryPlanDemo());
            var componentParamDemo = JsonConvert.SerializeObject(GetComponentParamsDemo());
            var pageRenderEngine=new BaikePageEngine(pageContext);
            pageRenderEngine.Load();
            return View(pageRenderEngine);
        }


        public ActionResult ComplexTemplate()
        {
            string boName = Convert.ToString(Request.Params["boname"]);
            if (string.IsNullOrWhiteSpace(boName))
                boName = "well-03";//复杂模板

            PageContext pageContext = new PageContext();
            pageContext.AddContextParam("instance", boName);
            pageContext.AddContextParam("well", boName);
            pageContext.AddContextParam("botype", BoType.Well);
            pageContext.AddContextParam("instanceclass", GetBoInstanceClassName(BoType.Well));

            var queryParamDemo = JsonConvert.SerializeObject(GetQueryPlanDemo());
            var componentParamDemo = JsonConvert.SerializeObject(GetComponentParamsDemo());
            var pageRenderEngine = new BaikePageEngine(pageContext);
            pageRenderEngine.Load();
            return View("index",pageRenderEngine);
        }

        public ActionResult Demo()
        {
            string boName = Convert.ToString(Request.Params["boname"]);
            if (string.IsNullOrWhiteSpace(boName))
                boName = "well-03";//复杂模板

            //PageContext pageContext = new PageContext();
            //pageContext.AddContextParam("instance", boName);
            //pageContext.AddContextParam("well", boName);
            //pageContext.AddContextParam("botype", BoType.Well);
            //pageContext.AddContextParam("instanceclass", GetBoInstanceClassName(BoType.Well));
            //var pageRenderEngine = new BaikePageEngineDemo(pageContext);
            //pageRenderEngine.Load();
            return View();
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

        public List<ComponentParam> GetComponentParamsDemo()
        {
            //是否显示更多
            ComponentParam param=new ComponentParam();
            param.Code = "show";
            param.Value = false;
            return new List<ComponentParam>(){ param };
        }



    }
}