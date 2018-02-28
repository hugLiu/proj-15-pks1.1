using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PKS.WITSML;
using System.Web.Mvc;
using PKS.WITSML.Model;

namespace PKS.SZXT.Web.Controllers
{
    public class WitsmlDataAccessorController : SZXTBaseController
    {
        /// <summary>
        /// 获取录井气测参数数据
        /// </summary>
        /// <param name="uidWell"></param>
        /// <param name="lastDateTime">该井上次最后数据时间</param>
        /// <returns></returns>
        public ActionResult GetMudLogDatas(string uidWell, DateTime lastDateTime)
        {
            return Json(DataAccess.GetMudLogDatas(uidWell, lastDateTime), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取录井气测参数数据
        /// </summary>
        /// <param name="uidWell"></param>
        /// <param name="lastDateTime">该井上次最后数据时间</param>
        /// <returns></returns>
        public ActionResult GetMudLogDatasByCount(string uidWell, int count)
        {
            var datas = DataAccess.GetMudLogDatas(uidWell, DateTime.MinValue).OrderByDescending(log => log.DateTime).Take(count);
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有随钻测井情况数据
        /// </summary>
        /// <param name="uidWell"></param>
        /// <param name="lastDateTime">该井上次最后数据时间</param>
        /// <returns></returns>
        public ActionResult GetLWDDatas(string uidWell, DateTime lastDateTime)
        {
            return Json(new List<LWD>(), JsonRequestBehavior.AllowGet);
            //return DataAccess.GetLWDDatas(uidWell, lastDateTime);
        }
        /// <summary>
        /// 获得所有井的uid列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllWells()
        {
            return Json(DataAccess.GetAllWells(), JsonRequestBehavior.AllowGet);
        }
    }
}