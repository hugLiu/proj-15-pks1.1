using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Messages;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebSchedule;

namespace Jurassic.WebTemplate.Controllers
{
    public class MessageDemoController : BaseController
    {
        //
        // GET: /MessageDemo/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(JMessage msg)
        {
            msg.SendToIds = CommOp.ToIntArray(Request.Form["SendTo"], ',');
            msg.Channel = (SendChannel)CommOp.ToIntArray(Request.Form["Channel"], ',').Sum();
            msg.SenderId = CurrentUserId.ToInt();
            SiteManager.Message.Send(msg);
            //msg.SendToIds.Each(id => SiteManager.Message.AlertFast(id));
            return JsonTips("success", "消息发送成功！");
        }

        public ActionResult UsersList()
        {
            var users = AppManager.Instance.UserManager.GetAll()
                .Select(u => new
                {
                    Id = u.Id,
                    Name = u.Name + "(" + u.Email + ")",
                });

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
