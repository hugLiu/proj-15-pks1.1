using PKS.Models;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.Web;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;
using PKS.Utils;

namespace PKS.SZXT.Web.Controllers
{
    public class ExplorationDecisionController : SZXTBaseController
    {
        // GET: ExplorationDecision
        public ActionResult ExplorationPlanning()
        {
            return View();
        }
        public string GetExplorationPlanningDataByYear(int beginYear, int endYear)
        {
            var service = GetService<IExplorationPlanningService>();
            service.SearchConfig = SearchConfig;
            var sBeginYear = beginYear.ToString();
            var sEndYear = endYear.ToString();
            var g1 = service.GetIndexData("G1", beginYear, endYear);
            var g2 = service.GetIndexData("G2", beginYear, endYear);
            var g3 = service.GetIndexData("G3", beginYear, endYear);
            var g4 = service.GetIndexData("G4", beginYear, endYear);
            var g5 = service.GetIndexData("G5", beginYear, endYear);
            var g6 = service.GetIndexData("G6", beginYear, endYear);
            var g7 = service.GetIndexData("G7", beginYear, endYear);
            var g8 = service.GetIndexData("G8", beginYear, endYear);
            //var g10 = service.GetIndexAppDataByQuery("G10", beginYear, endYear);
            var g11 = service.GetIndexData("G11", beginYear, endYear);
            var g12 = service.GetIndexData("G12", beginYear, endYear);
            var g13 = service.GetIndexDataAsNews("G13", beginYear, endYear, 10);
            var g14 = service.GetIndexData("G14", beginYear, endYear);
            var g15 = service.GetIndexData("G15", beginYear, endYear);
            //var g16 = service.GetIndexAppDataByQuery("G16", beginYear, endYear);
            //var g17 = service.GetIndexAppDataByQuery("G17", beginYear, endYear);
            //var g18 = service.GetIndexAppDataByQuery("G18", beginYear, endYear);
            var g19 = service.GetIndexDatasByQuery("G19", new string[] { sBeginYear, sEndYear }, false);
            var g20 = service.GetIndexData("G20", beginYear, endYear);
            var g21 = service.GetIndexData("G21", beginYear, endYear);
            var model = new
            {
                g1 = g1,
                g2 = g2,
                g3 = g3,
                g4 = g4,
                g5 = g5,
                g6 = g6,
                g7 = g7,
                g8 = g8,
                //g10 = g10,
                g11 = g11,
                g12 = g12,
                g13 = g13,
                g14 = g14,
                g15 = g15,
                //g16 = g16,
                //g17 = g17,
                //g18 = g18,
                g19 = g19,
                g20 = g20,
                g21 = g21
            };
            return SerializeObject(model);
        }
        public ActionResult AnnualPlan()
        {
            return View();
        }
        public string GetAnnualPlanDataByYear(string year)
        {
            string annual = year.IsNullOrEmpty() ? DateTime.Now.Year.ToString() : year;
            var service = GetService<IAnnualPlanService>();
            service.SearchConfig = SearchConfig;
            var g1 = service.GetSamplingWorkload(annual);
            var g2 = service.GetProcessingWorkload(annual);
            var g3 = service.GetWellWorkload(annual);
            var g4 = service.GetMainResearchEffort(annual);
            var g5 = service.GetExplorationBudget(annual);
            var g6 = service.GetSummaryOfGeoReserves(annual);
            var g7 = service.GetExplorationProductionMap(annual);
            var g8 = service.GetExplorationProductionTable(annual);
            var g9 = service.GetSummaryReport(annual, 8);
            var g10 = service.GetExplorationDispositionMap(annual);
            var g11 = service.GetSelfExplorationDeployment(annual);
            var g12 = service.GetCooperativeExplorationDeployment(annual);
            var g13 = service.GetTaskBookForExploration(annual, 8);
            //var g14 = service.GetAnnualPlanForExploration(annual, 8);
            var model = new
            {
                g1 = g1,
                g2 = g2,
                g3 = g3,
                g4 = g4,
                g5 = g5,
                g6 = g6,
                g7 = g7,
                g8 = g8,
                g9 = g9,
                g10 = g10,
                g11 = g11,
                g12 = g12,
                g13 = g13,
                //g14 = g14
            };
            return SerializeObject(model);
        }

        public ActionResult WellReview()
        {
            return View();
        }

        public ActionResult ExplorationDeployment()
        {
            return View();
        }
        public string GetWellLocationData(string year)
        {
            var service = GetService<IExplorationDeploymentService>();
            service.SearchConfig = SearchConfig;
            var g1 = service.GetWellLocationImg(year);
            var g2 = service.GetIndexDataByQuery("G2", new string[] { year }, false);
            var g3 = service.GetWellsiteSurveyReport(year, 10);
            var g4 = service.GetLocationProposal(year, 10);
            var g5 = service.GetSeismicDeploymentImg(year);
            var g6 = service.GetSeismicAcquisitionTable(year);
            var g7 = service.GetSeismicWorkAreaCensus(year, 10);
            var g8 = service.GetSeismicRecommendation(year, 10);
            var model = new
            {
                g1 = g1,
                g2 = g2,
                g3 = g3,
                g4 = g4,
                g5 = g5,
                g6 = g6,
                g7 = g7,
                g8 = g8
            };
            return SerializeObject(model);
        }
    }
}