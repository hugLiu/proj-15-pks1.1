using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PKS.Sooil.Controllers
{
    public class DemoController: SooilBaseController
    {
       // [ActionAllowOrigin(AllowSites = new string[] { "192.168.1.119","127.0.0.1", "http://localhost:33332" })]
        public ActionResult GetData()
        {
            int offset = Request.Params["offset"].ToInt();
            int limit = Request.Params["limit"].ToInt();

            string sortField = Request.Params["sort"].ToStr();
            string order = Request.Params["order"].ToStr();

            bool isClientPage = Request.Params["clientpage"].ToBool();
            var datas = GetAllDatas();

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (order == "asc")
                {
                    datas = datas.AsQueryable().OrderBy(sortField).ToList();
                }
                else
                {
                    datas = datas.AsQueryable().OrderBy(sortField, true).ToList();
                }
            }
            var totalCount = datas.Count();
            //若limit为0视为不分页，数据全部返回
            if (limit == 0|| isClientPage)
                return Json(datas, JsonRequestBehavior.AllowGet);
            //服务端分页，带上total
            return Json(
                new
                {
                    total = totalCount,
                    rows = datas.Skip(offset).Take(limit)
                }
                , JsonRequestBehavior.AllowGet);
        }

        public Expression<Func<T, string>> GetLambdaExpression<T>(string propertyName) where T:class
        {
            var paramExpress = Expression.Parameter(typeof(T), "item");
            var propertyExpression = Expression.Property(paramExpress, propertyName);
            return Expression.Lambda<Func<T, string>>(propertyExpression, paramExpress);
        }

        public List<DataItem> GetAllDatas()
        {
            List<DataItem> dataItems=new List<DataItem>();
            for (int i = 0; i < 30; i++)
            {
                dataItems.Add(new DataItem()
                {
                    name="名称"+(i+1),
                    address = "地址"+(30-(i+1)),
                    createdtime = DateTime.Now.ToString(),
                    username = "用户名"+(30-(i+1))
                });
            }
            return dataItems;
        }
    }

    public static class SimpleExtension
    {
        public static int ToInt(this object obj)
        {
            if (obj == null)
                return 0;
            int result = 0;
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        public static string ToStr(this object obj)
        {
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }

        public static bool ToBool(this object obj)
        {
            if (obj == null)
                return false;
            bool result = false;
            bool.TryParse(obj.ToString(), out result);
            return result;
        }
    }
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return OrderBy(queryable, propertyName, false);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool desc)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Property(param, propertyName);
            dynamic keySelector = Expression.Lambda(body, param);
            return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
        }
    }

    public class DataItem
    {
    
        public string name { get; set; }
        public string createdtime { get; set; }
        public string username { get; set; }
        public string address { get; set; }
    }
}