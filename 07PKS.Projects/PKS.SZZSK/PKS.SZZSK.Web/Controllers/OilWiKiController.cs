using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PKS.DbModels.OilWiki;
using PKS.DbServices.OilWiki;
using PKS.DbServices.OilWiki.Model;
using PKS.Utils;

namespace PKS.SZZSK.Web.Controllers
{
    public class OilWiKiController : SZZSKController
    {

        private OilWikiService _oilWikiService;
        public OilWiKiController(OilWikiService oilWikiService)
        {
            _oilWikiService = oilWikiService;
        }
        // GET: OilWiKi
        public ActionResult Index()
        {
            ViewBag.Catalogs = GetCatalogs();//GetCatalog();
            return View();
        }

        public ActionResult Navigation()
        {
            return View(GetCatalogs());
        }

        public JsonResult GetSearch(string query)
        {
            var searchResult = _oilWikiService.SearchEntry(query).Select(e => new { id = e.Id, name = e.NAME });
            return Json(searchResult);
        }
        public ContentResult GetEntryByName(string name)
        {
            var searchResult = _oilWikiService.GetEntryByName(name);
            var id = searchResult == null ? -1 : searchResult.Id;
            return Content(id.ToString());
        }
        private IOrderedEnumerable<KeyValuePair<PKS_OILWIKI_CATALOG, IOrderedEnumerable<PKS_OILWIKI_CATALOG>>> GetCatalog()
        {
            var catlogs = _oilWikiService.GetCatalog().ToList();
            return (from parent in catlogs
                    join child in catlogs
                    on parent.Id equals child.PARENTID
                    where parent.LEVELNUMBER == 1 && child.LEVELNUMBER == 2
                    select new { parent = parent, child = child })
                    .GroupBy(a => a.parent)
                    .ToDictionary(k => k.Key, v => v.Select(c => c.child).OrderBy(n => n.ORDERNUMBER))
                    .OrderBy(o => o.Key.ORDERNUMBER);
        }
        private IEnumerable<PKS_OILWIKI_CATALOG> GetCatalogs()
        {
            var catlogs = _oilWikiService.GetCatalog().ToList();
            return catlogs;
        }
        public ActionResult List(int id = 0, int from = 0, int size = 16)
        {
            string catelogName = string.Empty;
            string catelogId = "0";
            int total = 0;
            var items = new List<EntryModel>();
            var entrys = _oilWikiService.GetEntryByCatalogId(id).ToList();

            foreach(var item in entrys)
            {
                item.CONTENTS = HttpUtility.HtmlDecode(item.CONTENTS);
            }
            if (entrys != null && entrys.Count > 0)
            {
                total = entrys.Count();
                catelogName = entrys[0].PKS_OILWIKI_CATALOG.NAME;
                catelogId = entrys[0].PKS_OILWIKI_CATALOG.Id.ToString();
                items = entrys.Select(t => new EntryModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    Image = t.IMAGE,
                    Contents = t.CONTENTS
                }).Skip(from).Take(size)?.ToList();
            }
            ViewBag.SearchId = id;
            ViewBag.CatelogName = catelogName;
            ViewBag.CatelogId = catelogId;
            ViewBag.Model = new
            {
                items,
                total
            }.ToJson();
            return View();
        }

        public JsonResult PageList(int id = 0, int from = 0, int size = 16)
        {
            int total = 0;
            var entrys = _oilWikiService.GetEntryByCatalogId(id)
                ?.Select(t => new EntryModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    Image = t.IMAGE,
                    Contents = t.CONTENTS
                }).ToList();
            foreach (var item in entrys)
            {
                item.Contents = HttpUtility.HtmlDecode(item.Contents);
            }

            if(entrys!=null && entrys.Count() > 0)
            {
                total = entrys.Count();
                entrys = entrys.Skip(from).Take(size)?.ToList();
            }

            var model = new
            {
                Data = entrys,
                Total = total
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Entry(string iiid, string dataid, int id = 0)
        {
            //int index = 0;
            //bool behaviorLog = false;
            //if (string.IsNullOrEmpty(iiid))
            //{
            //    index = id;
            //    behaviorLog = true;
            //}
            //else
            //{
            //    index = int.Parse(dataid);
            //}

            //var entry = _oilWikiService.GetEntryById(index);

            //if (behaviorLog)
            //{
            //    //记录行为日志
            //    var resourcekey = $"勘探知识库/石油百科/{entry.ParentCatalogName}/{entry.Id}";
            //    string entryiiid = resourcekey.ToMD5();
            //    ReocrdUserebavior(entryiiid);
            //}
            EntryDetails entryDetail = null;
            if (id> 0)
            {
                entryDetail = _oilWikiService.GetEntryById(id);
                var resourcekey = $"勘探知识库/石油百科/{entryDetail.ParentCatalogName}/{entryDetail.Id}";
                string entryiiid = resourcekey.ToMD5();
                ReocrdUserebavior(entryiiid);
            }
            else
            {
                entryDetail = _oilWikiService.GetEntryById(int.Parse(dataid));
            }
            return View(entryDetail);
        }
    }
}