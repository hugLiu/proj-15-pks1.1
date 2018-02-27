using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebSchedule;
using Jurassic.CommonModels;

namespace Jurassic.WebTemplate.Controllers
{
    public class SignalRDemoController : BaseController
    {
        static SignalRDemoController()
        {
            SignalRProcesserFactory.Instance.Register("简单抓取", new SimpleUrlSpiderProcesser());
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.jurassic.com.cn");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.sina.com.cn");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.sohu.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.qq.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.baidu.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.163.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.taobao.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.yahoo.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.tom.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.tianya.cn");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.linkedin.com");
            SignalRProcesserFactory.Instance.Add("简单抓取", "http://www.csdn.net/");

            SignalRProcesserFactory.Instance.Register("简单延时", new DummyProcesser());
            SignalRProcesserFactory.Instance.Add("简单延时", 1000);
            SignalRProcesserFactory.Instance.Add("简单延时", 2000);
            SignalRProcesserFactory.Instance.Add("简单延时", 300);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 2000);
            SignalRProcesserFactory.Instance.Add("简单延时", 300);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 2000);
            SignalRProcesserFactory.Instance.Add("简单延时", 300);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 2000);
            SignalRProcesserFactory.Instance.Add("简单延时", 300);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 300);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
            SignalRProcesserFactory.Instance.Add("简单延时", 500);
        }
        //
        // GET: /SignalR/

        public ActionResult Index()
        {
            // SignalRProcesserFactory.Instance.ClearAll();

            return View();
        }

        public void Start(string processerName)
        {
            SignalRProcesserFactory.Instance.Start(processerName);
        }

        public void Broadcast(string text)
        {
            SignalRProcesserFactory.Instance.Add("Broadcast", User.Identity.Name + ": " + text);
        }

        public void AlertFast(int userId)
        {
            SiteManager.Message.AlertFast(userId, DateTime.Now.Second);
        }

        public void AlertFullFast(int userId)
        {
            SiteManager.Message.AlertFast(userId);
        }

    }

    class SimpleUrlSpiderProcesser : ProcesserBase
    {
        public override void Process(object item)
        {
            string str = WebHelper.GetWebResponseText(item.ToString());
        }
    }

    class DummyProcesser : ProcesserBase
    {
        public override void Process(object item)
        {
            System.Threading.Thread.Sleep((int)item);
        }
    }
}
