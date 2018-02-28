using PKS.DbServices;
using PKS.DbServices.KManage.Model;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class KManage2Controller : PKSBaseController
    {
        private KManage2Service _kManage2Service;
        private IBO2Service _bo2Service;
        private IIndexerService _indexService;

        public KManage2Controller(KManage2Service kManage2Service)
        {
            _kManage2Service = kManage2Service;
            _bo2Service = GetService<IBO2Service>();
            _indexService = GetService<IIndexerService>();
        }

        // GET: KManage2
        public ActionResult PageManage()
        {
            return View();
        }

        public ActionResult InstanceManage()
        {
            return View();
        }

        #region PageManage

        public JsonResult GetKtemplateTree()
        {
            return Json(_kManage2Service.GetTemplateTree(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParameterTree()
        {
            return Json(_kManage2Service.GetParameterTree(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstanceClass()
        {
            FilterRequest request = new FilterRequest
            {
                Query = new { },
                Fields = new { },
                Sort = new { }
            };
            var bots = _bo2Service.FilterBOTs(request);
            return Json(bots.Select(t => new { id = t.Name, text = t.Name }), JsonRequestBehavior.AllowGet);
        }

        

        public JsonResult GetSubSystems()
        {
            return Json(_kManage2Service.GetSubSystems(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUrls()
        {
            return Json(_kManage2Service.GetUrls(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplates(string id)
        {
            if (string.IsNullOrEmpty(id)) id = "1";
            return Json(_kManage2Service.GetTemplates(id.ToInt32()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPageManageData(string id)
        {
            if (string.IsNullOrEmpty(id)) id = "1";
            return Json(_kManage2Service.GetPageManageData(id.ToInt32()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePageManage(string id, 
                                   string SubSystemId, 
                                   string InstanceClass, 
                                   string GroupName,
                                   string RObject,
                                   string UrlId,
                                   string DefaultTempId,
                                   string Params)
        {
            PageManageModel pageManageModel = new PageManageModel();
            pageManageModel.SubSystemId = SubSystemId.ToInt32();
            pageManageModel.InstanceClass = InstanceClass;
            pageManageModel.UrlId = UrlId.ToInt32();
            pageManageModel.DefaultTempId = DefaultTempId.ToInt32();
            pageManageModel.GroupName = GroupName;
            pageManageModel.RObject = RObject;
            if (!string.IsNullOrEmpty(Params))
                pageManageModel.Params = Params.JsonTo<List<ComboItem>>();
            _kManage2Service.SavePageManage(id.ToInt32(), pageManageModel);
            return Json(new { defaultId = pageManageModel.DefaultTempId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region InstanceManage

        public JsonResult GetTemplateInfo(string tid)
        {
            return Json(_kManage2Service.GetTemplateInfo(tid.ToInt32()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBosByBot(string bot)
        {
            var result = new List<object>();
            FilterRequest request = new FilterRequest
            {
                Query = new { bot = bot },
                Fields = new { bo = 1 },
                Sort = new { boid = 1 }
            };
            List<BO2> bos = _bo2Service.FilterBOs(request);
            if (bos.Count > 0)
            {
                result.AddRange(bos.Select(t => new 
                {
                    id = t.BO,
                    text = t.BO
                }).OrderBy(t => t.id)
                .ToList());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstances(string tid)
        {
            var templateId = tid.ToInt32();
            var result = _kManage2Service.GetInstances(templateId);
            var tempInfo = _kManage2Service.GetTemplateInfo(templateId);
            if (tempInfo.IsDefault)
            {
                // 通过对象服务获取对象列表
                FilterRequest request = new FilterRequest
                {
                    Query = new { bot = tempInfo.InstanceClass },
                    Fields = new { bo = 1},
                    Sort = new { boid = 1 }
                };
                List<BO2> bos = _bo2Service.FilterBOs(request);
                if (bos.Count > 0)
                {
                    var list = _kManage2Service.FilterInstances(bos.Select(t => t.BO).ToList());
                    foreach(var item in list)
                    {
                        result.Add(new KInstance
                        {
                            Id = 0,
                            Instance = item,
                            KTemplateId = templateId,
                            InstanceClass = tempInfo.InstanceClass
                        });
                    }
                }
                
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptions(string bot, string pname)
        {
            var result = new List<string>();
            // 通过对象服务获取对象列表
            var botItem = _bo2Service.GetBOT(bot);
            if (botItem != null)
            {
                var property = botItem.Properties.FirstOrDefault(t => t.Name.Equals(pname));
                if (property != null) result.AddRange(property.Options);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectBo(string bot, string templateId)
        {
            if (string.IsNullOrEmpty(bot)) return null;
            var botItem = _bo2Service.GetBOT(bot);
            if (botItem == null) return null;
            ViewData["model"] = botItem;
            ViewData["templateId"] = templateId.ToInt32(); 
            ViewData["bolist"] = _kManage2Service.GetInstancesByClass(bot); 
            return View();
        }

        public JsonResult QueryBo(string bot, string model)
        {
            var bolist = _kManage2Service.GetInstancesByClass(bot);
            var result = new List<object>();
            string filter = "{\"bot\":" + "\"" + bot + "\",";
            var dict = new Dictionary<string, List<string>>();
            var list = new List<NameValue>();
            if (!string.IsNullOrEmpty(model)) list = model.JsonTo<List<NameValue>>();
            foreach(var item in list)
            {
                if (!dict.ContainsKey(item.name))
                {
                    dict.Add(item.name, new List<string>());
                }
                dict[item.name].Add(item.value);
            }
            foreach(var key in dict.Keys)
            {
                filter += "\"" + "properties." + key + "\":{$in:[";              
                foreach (var item in dict[key])
                {
                    filter += "\"" + item + "\",";
                }
                filter = filter.TrimEnd(',');
                filter += "]},";
            }
            filter = filter.TrimEnd(',');
            filter += "}";

            FilterRequest request = new FilterRequest
            {
                Query = filter.JsonTo(),
                Fields = new { bo = 1 },
                Sort = new { boid = 1 }
            };
            List<BO2> bos = _bo2Service.FilterBOs(request);
            if (bos.Count > 0)
            {
                result.AddRange(bos.Select(t => new
                {
                    id = bolist.Contains(t.BO) ? "是" : "否",
                    text = t.BO
                }).ToList());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void SaveInstances(string tid, string bot, string pushlist)
        {
            _kManage2Service.SaveInstances(tid.ToInt32(), bot, pushlist.JsonTo<List<string>>(), CurrentUser.Name);
        }

        public bool HasTemplate(string tid, string instance, string instanceClass)
        {
            var bHas =  _kManage2Service.HasTemplate(tid.ToInt32(), instance, instanceClass);
            if (bHas) return bHas;
            else
            {
                var pushlist = new List<string>();
                pushlist.Add(instance);
                _kManage2Service.SaveInstances(tid.ToInt32(), instanceClass, pushlist, CurrentUser.Name);
                return bHas;
            }
        }

        public void DeleteInstance(string id)
        {
            if (id.ToInt32() != 0)
                _kManage2Service.DeleteInstance(id.ToInt32());
        }

        public void StaticInstance(string instance, string tid)
        {

        }

        #endregion

        #region Create Index

        public int IndexTargetBaiKe(string category, int categoryid, string bot,string pageid, string groupname, string robject)
        {
            int indexCount = 0;
            FilterRequest request = new FilterRequest
            {
                Query = new { bot = bot },
                Fields = new { bo = 1 },
                Sort = new { boid = 1 }
            };
            var botData = _bo2Service.GetBOT(bot);
            if (botData == null) return indexCount;
            var bos = _bo2Service.FilterBOs(request);
            if (bos != null && bos.Count > 0)
            {
                MetadataCollection collection = new MetadataCollection();
                // 获取默认模板
                var defaultTemplate = _kManage2Service.GetDefaultTemplate(categoryid);
                foreach (var bo in bos)
                {
                    // 获取bo的模板，如果没有，获取默认模板
                    var template = _kManage2Service.GetInstanceTemplate(bo.BO, bot);
                    if (template == null) template = defaultTemplate;
                    if (template == null) template = category;
                    // 获取目录字符串
                    var catalogueStr = _kManage2Service.GetCatalogueStr(template);

                    Metadata metadata = new Metadata();
                    var resourcekey = "勘探知识库\\" + category + "\\" + bo.BO;
                    metadata.IIId = resourcekey.ToMD5();
                    metadata.IndexedDate = DateTime.Now;
                    metadata.Thumbnail = null;
                    metadata.Fulltext = catalogueStr;
                    metadata.PageId = pageid;
                    metadata.DataId = bo.BO;
                    metadata["dsn"] = "勘探知识库";
                    metadata.ShowType = IndexShowType.Mixing.ToString();
                    metadata["title"] = bo.BO + robject + groupname;
                    metadata["subject"] = null;
                    metadata["abstract"] = null;
                    metadata["catalogue"] = catalogueStr;
                    metadata["author"] = null;
                    metadata["submitter"] = null;
                    metadata["auditor"] = null;
                    metadata["createddate"] = DateTime.Now;
                    metadata["submitteddate"] = null;
                    metadata["auditteddate"] = null;
                    metadata["status"] = "已审核";
                    metadata["frequency"] = null;
                    metadata["period"] = null;
                    metadata["basin"] = null;
                    metadata["firstlevel"] = null;
                    metadata["secondlevel"] = null;
                    metadata["trap"] = null;
                    metadata["well"] = null;
                    metadata["swa"] = null;
                    metadata["miningarea"] = null;
                    metadata["cozone"] = null;
                    metadata["project"] = null;
                    metadata["pc"] = category;
                    metadata["pt"] = template;
                    metadata["bd"] = "勘探";
                    metadata["bt"] = groupname;
                    metadata["bp"] = robject;
                    metadata["ba"] = null;
                    metadata["bf"] = null;
                    metadata["system"] = "勘探知识库";
                    metadata["resourcetype"] = "勘探知识库\\" + category;                   
                    metadata["resourcekey"] = resourcekey;

                    metadata[botData.Code] = bo.BO;

                    collection.Add(metadata);                    
                }
                IndexSaveRequest indexrequest = new IndexSaveRequest()
                {
                    Replace = true,
                    Metadatas = collection
                };
                var iiids = _indexService.Save(indexrequest);
                indexCount = iiids.Count();
            }
            return indexCount;
        }

        #endregion
    }

    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}