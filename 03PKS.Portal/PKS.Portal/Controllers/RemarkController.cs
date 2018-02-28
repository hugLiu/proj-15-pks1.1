using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.DbModels;
using PKS.DbModels.Portal;
using PKS.DbServices.Portal.Remark;
using PKS.DbServices.Portal.Remark.Model;

namespace PKS.Portal.Controllers
{
    public class RemarkController : PortalBaseController
    {
        public RemarkService _remarkService { get; set; }

        public RemarkController(RemarkService remarkService)
        {
            _remarkService = remarkService;
        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户信息的方法
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserInfo()
        {
            
            //.net获取用户信息的方法
            //var user = User.Identity.Name;
            //框架在BaseControlleer的对象
            // var users = CurrentUser.Id;
            //var user = new UserProfileModel
            //{
            //    UserId = Convert.ToInt32(CurrentUser.Id),
            //    UserName = CurrentUser.Name
            //};
            return Json(new
            {
                UserId= CurrentUser.USERID,
                UserName=CurrentUser.USERNAME
            }, JsonRequestBehavior.AllowGet);
        }

        public USERPROFILE CurrentUser
        {
            get
            {
                return new USERPROFILE()
                {
                    USERID = Convert.ToInt32(PKSUser.Identity.Id),
                    USERNAME =PKSUser.Identity.Name
                };
            }
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="scoap"></param>
        /// <param name="naturekey"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult GetRemarkList(string iiid, string index, string size, string filter)
        {
            var dataList = _remarkService.QueryRemark(iiid, Convert.ToInt32(index), Convert.ToInt32(size), filter, Convert.ToInt32(CurrentUser.USERID));
            if(dataList.data!=null)
                dataList.data.ForEach(item=>item.UserPhotoUrl="/content/images/avatar/default.png");
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        private string ToJson(object value, bool indent = true)
        {
            if (value == null) return null;
            Type type = value.GetType();
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            StringWriter sw = new StringWriter();
            Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw);
            writer.Formatting = indent ? Formatting.Indented : Formatting.None; // Formatting.Indented;
            if (indent)
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                json.Converters.Add(timeFormat);
            }
            else
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

                //var timeFormat = new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter();
                json.Converters.Add(timeFormat);
            }
            writer.QuoteChar = '"';
            json.Serialize(writer, value);

            string output = sw.ToString();
            writer.Close();
            return output;
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        public void DeleteRemark(string Id)
        {
            _remarkService.DeleteRemark(Convert.ToInt32(Id));
        }
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="model"></param>
        public void AddRemark(RemarkModel model)
        {
            model.CreatedBy = CurrentUser.USERNAME;
            model.CreatedDate = DateTime.Now;
            model.LastUpdatedDate = DateTime.Now;
            model.LastUpdatedBy = CurrentUser.USERNAME;
            _remarkService.AddRemark(model);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="praised"></param>
        public void PraiseRemark(int id, string userId, string praised)
        {
            PKS_Remark_Thumbup thumbup = new PKS_Remark_Thumbup();
            thumbup.RemarkId = id;
            thumbup.UserId = Convert.ToInt32(userId);
            thumbup.CreatedBy = CurrentUser.USERNAME;
            thumbup.CreatedDate = DateTime.Now;
            thumbup.LastUpdatedBy= CurrentUser.USERNAME;
            thumbup.LastUpdatedDate = DateTime.Now;
            _remarkService.PraiseRemark(thumbup, Convert.ToBoolean(praised));
        }
    }
}