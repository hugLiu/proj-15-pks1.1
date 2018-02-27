using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter.Resources;
using System.Data;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Logs;
using Jurassic.AppCenter;
using System.Globalization;

namespace Jurassic.WebFrame.Controllers
{
    /// <summary>
    /// 资源管理的控制器
    /// </summary>
    [JAuth(Name = "ResourcesManager", Ord = 4)]
    public class ResController : BaseController
    {
        //
        // GET: /AppCenter/Res/

        /// <summary>
        /// 资源管理的首页
        /// </summary>
        /// <returns>首页</returns>
        public ActionResult Index()
        {
            var keys = ResHelper.GetUsedCultureNames();
            return View(keys);
        }

        /// <summary>
        /// 获取所有资源的DataTable
        /// 修改时间：2017-01-04，修改人：汪敏
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAll(string key = null)
        {
            //查询关键字放入Session
            Session["LanguageQueryKey"] = !key.IsEmpty() ? key : "";

            DataView dv = ResHelper.MakeDataTable().AsDataView();

            try
            {
                if (!key.IsEmpty())
                {
                    //语言key的过滤条件
                    string condition = String.Format("Key LIKE '%{0}%'", key);

                    //增加每一个语种的过滤
                    foreach (string culture in ResHelper.GetUsedCultureNames())
                    {
                        condition = condition + " or " + String.Format("[" + culture + "] LIKE '%{0}%'", key);
                    }

                    dv.RowFilter = condition;

                }
            }
            catch(Exception ex)
            {
                string errinfo = ex.ToString();
            }

            return JsonNT(dv.ToTable());
        }

        /// <summary>
        /// 保存所有应用程序运行中产生的资源信息
        /// 以便于今后可以手工编辑缺失的资源内容
        /// 修改时间：2017-01-04，修改人：汪敏
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        [JAuth(Name = "Edit")]
        public ActionResult Index(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                
                //整体提交的数据
                var data = Request.Form["data"];
                DataTable dt = JsonHelper.FromJson<DataTable>(data);
                RestoreDataTable(dt);

                //提交的数据要判断key是否空字符串
                bool isKeyNull = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Key"] == null)
                    {
                        isKeyNull = false;
                        break;
                    }
                    if (dr["Key"].ToString().Trim().Count() == 0)
                    {
                        isKeyNull = false;
                        break;
                    }
                }
                if (isKeyNull == false)
                {
                    return JsonTipsLang("error", "Language_Key_IsNull");
                }

                //提交的数据要判断每一个语种数据列是否存在，如果不存在，需要补上
                if (!dt.Columns.Contains("Key"))
                {
                    dt.Columns.Add("Key", typeof(String));
                }
                foreach (string culture in ResHelper.GetUsedCultureNames())
                {
                    if (!dt.Columns.Contains(culture))
                    {
                        dt.Columns.Add(culture, typeof(String));
                    }
                }

                //从Session取查询关键字
                var queryKey = Session["LanguageQueryKey"] as string;
                if (!queryKey.IsEmpty())
                {
                    DataView dv = ResHelper.MakeDataTable().AsDataView();

                    //语言key的过滤条件
                    string condition = String.Format("Key NOT LIKE '%{0}%'", queryKey);
                    //增加每一个语种的过滤
                    foreach (string culture in ResHelper.GetUsedCultureNames())
                    {
                        condition = condition + " and " + String.Format("[" + culture + "] NOT LIKE '%{0}%'", queryKey);
                    }
                    dv.RowFilter = condition;

                    DataTable existeddt = dv.ToTable();
                    //与提交的数据合并
                    foreach (DataRow dr in existeddt.Rows)
                    {
                        dt.Rows.Add(dr.ItemArray);
                    }
                }

                //保存数据
                ResHelper.SaveDataTable(dt);
                return JsonTipsLang("success", "Resources_Saved");
            }
            return JsonTips();
        }

        //移除mini-ui整体提交时传过来的多余信息
        private DataTable RestoreDataTable(DataTable dt)
        {
            if (dt.Columns.Contains("_uid"))
            {
                dt.Columns.Remove("_uid");
            }
            if (dt.Columns.Contains("_index"))
            {
                dt.Columns.Remove("_index");
            }
            if (dt.Columns.Contains("_state"))
            {
                dt.Columns.Remove("_state");
            }
            ///在mini-ui升到3.6后，又多出了这个_Id
            if (dt.Columns.Contains("_id"))
            {
                dt.Columns.Remove("_id");
            }
            return dt;
        }

        /// <summary>
        /// 新增语种
        /// </summary>
        /// <param name="culture">语言简称</param>
        /// <returns></returns>
        public ActionResult AddCulture(string culture)
        {
            DataTable dt = ResHelper.MakeDataTable();
            dt.Columns.Add(culture);
            try
            {
                CultureInfo ci = new CultureInfo(culture);
            }
            catch
            {
                return JsonTips("error", "{0}_NotExist", (object)null, culture);
            }
            ResHelper.SaveDataTable(dt);
            return JsonTipsLang("success", "Language_Add_Success");
        }

        /// <summary>
        /// 删除语种
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ActionResult DeleteCulture(string culture)
        {
            DataTable dt = ResHelper.MakeDataTable();
            dt.Columns.Remove(culture);
            ResHelper.RemoveCulture(culture);
            ResHelper.SaveDataTable(dt);
            return JsonTipsLang("success", "Languae_Delete_Success");
        }

        /// <summary>
        /// 清除所有资源,所有资源文件变为空文件
        /// </summary>
        /// <returns>返回操作结果的Json提示信息</returns>
        [HttpPost]
        [JAuth(Name = "Clear+All+Resources")]
        public ActionResult ClearAllRes()
        {
            ResHelper.ClearAllRes();
            return null;
        }

        /// <summary>
        /// 接受一个新的Key到所有的语言
        /// </summary>
        /// <param name="key">Key</param>
        public void AddKey(string key)
        {
            ResHelper.AddKey(key);
        }

        /// <summary>
        /// 返回一个js对象以使前台脚本也可以调用后台JS
        /// 本方法将自动按照当前线程的区域特性返回对应的资源对象。
        /// </summary>
        /// <param name="id">生成的Js的对象名,默认叫JStr</param>
        /// <returns>一个js对象</returns>
        [JAuth(JAuthType.Ignore, LogType = JLogType.Debug)]
        [OutputCache(Duration = 600, VaryByParam = "id")]
        public string GetAllJsRes(string id = "JStr")
        {
            Response.ContentType = "text/javascript";
            return ResHelper.GetAllResStrJs(id);
        }
    }
}
