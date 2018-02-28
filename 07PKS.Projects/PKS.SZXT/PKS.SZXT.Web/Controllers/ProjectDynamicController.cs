using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;
using PKS.SZXT.IService.ProjectDynamic;

namespace PKS.SZXT.Web.Controllers
{
    public class ProjectDynamicController : SZXTBaseController
    {
        private static readonly int topNum = 8;
        // GET: ProjectDynamic
        public ActionResult OperationProject()
        {
            var svc = GetService<IOperationProject>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetProjectHeadlines(topNum);
            var g2_1 = svc.GetTopHots(topNum);
            var g2_2 = svc.GetRecentlyView(PKSUser.Identity.Name, topNum);
            var g3 = svc.GetProjectProgress();
            var g4 = svc.GetProjectProgress2();
            var g5 = svc.GetProjectManagement();
            var model = new
            {
                projectHeadlines = g1,
                topHots = g2_1,
                recentlyView = g2_2,
                projectProgress = g3,
                projectProgress2 = g4,
                projectManagement = g5
            };
            return View(model: SerializeObject(model));
        }

        public ActionResult ResearchProject()
        {
            var svc = GetService<IResearchProject>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetProjectHeadlines(topNum);
            var g2_1 = svc.GetTopHots(topNum);
            var g2_2 = svc.GetRecentlyView(PKSUser.Identity.Name, topNum);
            var g3 = svc.GetProjectProgress();
            var model = new
            {
                projectHeadlines = g1,
                topHots = g2_1,
                recentlyView = g2_2,
                projectProgress = g3,
            };
            return View(model: SerializeObject(model));
        }
        public string GetProjectDetail(string projectName)
        {
            var svc = GetService<IResearchProject>();
            svc.SearchConfig = SearchConfig;
            var g3_1 = svc.GetProjectApproval(projectName);
            var g3_2 = svc.GetProjectImplement(projectName);
            var g3_3 = svc.GetProjectAcceptance(projectName);

            var model = new
            {
                g3_1 = g3_1,
                g3_2 = g3_2,
                g3_3 = g3_3
            };

            return SerializeObject(model);
        }
    }
}