using Newtonsoft.Json.Linq;
using PKS.DbServices;
using PKS.DbServices.Semantic.Model;
using PKS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class SemanticsController : PKSBaseController
    {
        private SemanticsManageService _semanticsManageService;
        private List<TermTreeModel> _list = null;

        public SemanticsController(SemanticsManageService semanticsManageService)
        {
            _semanticsManageService = semanticsManageService;
        }

        #region 概念类维护

        //概念类维护
        public ActionResult ConceptClass()
        {
            return View();
        }

        public JsonResult GetConceptClassList()
        {
            var list = _semanticsManageService.GetConceptClassList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConceptClassAdd()
        {
            return View();
        }

        public ActionResult ConceptClassEdit(string model)
        {
            if (model == null) return null;
            var ccModel = model.JsonTo<ConceptClassItem>();
            ViewData["model"] = ccModel;
            return View();
        }

        public JsonResult UpdateConceptClass(string model, string state)
        {
            var data = new Dictionary<string, string>();
            var newClassModel = model.JsonTo<ConceptClassItem>();
            var operate = state.ToEnum<Operate>();
            try
            {
                _semanticsManageService.UpdateConceptClass(newClassModel, operate);
                data.Add("State", "success");
                if (operate == Operate.Create) data.Add("Text", "数据添加成功！");
                if (operate == Operate.Update) data.Add("Text", "数据修改成功！");
                if (operate == Operate.Delete) data.Add("Text", "数据删除成功！");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Add("State", "error");
                data.Add("Text", e.Message);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 关键字维护

        //关键字维护
        public ActionResult KeyWords()
        {
            return View();
        }

        // 加载数据BP和PT（以树的结构展示）按照orderindex排序，同时判断记录关键词录入状态
        public JsonResult GetBPAndPtTreeResult(string id, bool show)
        {
            return Json(_semanticsManageService.GetBPPTTree(id, show), JsonRequestBehavior.AllowGet);
        }

        // 根据id获得关键词的列表
        public JsonResult GetKeyWords(string id)
        {
            return Json(_semanticsManageService.GetKeyWordsById(id.ToInt32()), JsonRequestBehavior.AllowGet);
        }

        // 添加关键词
        public void UpdateKeyWords(string id, string words, string operatestr)
        {
            Operate operate = Operate.Create;
            if (operatestr == "Update") operate = Operate.Update;
            if (operatestr == "Delete") operate = Operate.Delete;

            var userName = CurrentUser.Name;
            words = Server.UrlDecode(words);
            var keywords = new Dictionary<string, int>();

            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                foreach (var token in tokens)
                {
                    var word = token["text"].ToString();
                    var order = Convert.ToInt32(token["id"].ToString());
                    //去除重复项
                    if (keywords.ContainsKey(word)) continue;
                    keywords.Add(word, order);
                }
            }
            _semanticsManageService.UpdateKeyWords(id.ToInt32(), userName, keywords, operate);
        }

        #endregion

        #region 叙词维护

        //叙词维护
        public ActionResult Glossary()
        {
            return View();
        }

        // 获取词来源
        public JsonResult GetSourceListOfCc(string strccterm)
        {
            if (strccterm.IsNullOrEmpty()) return null;
            var list = _semanticsManageService.GetSourceList(strccterm);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 根据叙词ID获取词来源
        public JsonResult GetSourceListById(string nodeId)
        {
            var newList = _semanticsManageService.GetSourceListById(nodeId.ToInt32());
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        // 获取翻译词
        public JsonResult GetTermTraslations(string termClassId)
        {
            var list = _semanticsManageService.GetTermTraslations(termClassId.ToInt32());
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 获取同义词（别名）
        public JsonResult GetTermAlias(string termClassId)
        {
            var list = _semanticsManageService.GetSemantics(termClassId.ToInt32(), "D", Direction.Forward);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 根据概念类获取叙词树
        public JsonResult GetCcTermResult(string strccterm)
        {
            strccterm = Server.UrlDecode(strccterm);
            var ccTermInfo = _semanticsManageService.GetSRByCC(strccterm);
            if (ccTermInfo == null) return null;
            var result = _semanticsManageService.GetCcTermTree(ccTermInfo.CCCode, ccTermInfo.SR);
            if (result != null) _list = result;
            return Json(_list, JsonRequestBehavior.AllowGet);
        }

        // 根据ID删除叙词
        public JsonResult DeleteCcTermByid(string id)
        {
            _semanticsManageService.DeleteCcTermById(id.ToInt32());
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 添加子节点
        public JsonResult AddChildNode(string termGuid, string model, string textterm)
        {
            var newmodel = model.JsonTo<CcTermModel>();
            newmodel.CreatedBy = newmodel.LastUpdatedBy = CurrentUser.Name;//正式测试的时候使用
            newmodel.CreatedDate = newmodel.LastUpdatedDate = DateTime.Now;
            var ccTermInfo = _semanticsManageService.GetSRByCC(textterm);
            newmodel.CCCode = ccTermInfo.CCCode;
            newmodel.TermClassID = _semanticsManageService.UpdateCcTerm(newmodel, Operate.Create);
            var ccTerm = _semanticsManageService.GetCcTermById(termGuid.ToInt32());
            string sr = _semanticsManageService.GetSRByCC(textterm).SR;
            if (ccTerm != null&&!string.IsNullOrEmpty(sr))
            {
                var newSemantics = new SemanticsModel()
                {
                    SR = sr, //概念类之间的关系
                    OrderIndex = newmodel.OrderIndex, //排序号
                    CreatedDate = newmodel.CreatedDate, //创建日期
                    CreatedBy = newmodel.CreatedBy, //创建人
                    FTermClassId = ccTerm.TermClassID,
                    FTerm = ccTerm.Term,
                    LTerm = newmodel.Term,
                    LTermClassId = newmodel.TermClassID,
                    LastUpdatedBy = newmodel.LastUpdatedBy, //最后更新人
                    LastUpdatedDate = newmodel.LastUpdatedDate, //最后更新时间
                    Remark = newmodel.Remark,
                };
                _semanticsManageService.UpdateSemantics(newSemantics, Operate.Create);
            }
            _list = _semanticsManageService.GetCcTermTree(textterm);
            return Json(_list, JsonRequestBehavior.AllowGet);
        }

        // 编辑节点
        public JsonResult EditTermById(string termGuid, string term, string path)
        {
            _semanticsManageService.EditCcTermTreeById(termGuid.ToInt32(), term, path);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 拖拽节点
        public JsonResult DragNodeResult(string sdModel, string dragmodel, string textValue)
        {
            var newSemantics = sdModel.JsonTo<SemanticsModel>();
            var newCcterm = dragmodel.JsonTo<CcTermModel>();
            newSemantics.LastUpdatedBy = newCcterm.LastUpdatedBy = CurrentUser.Name;
            newSemantics.LastUpdatedDate = newCcterm.LastUpdatedDate = DateTime.Now;         
            _semanticsManageService.DragNodeEf(newSemantics, newCcterm, textValue);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 保存描述
        public JsonResult SavsDesc(string strDesc, string termGuid)
        {
            var ccTerm = _semanticsManageService.GetCcTermById(termGuid.ToInt32());
            if (ccTerm != null) ccTerm.Description = strDesc;
            _semanticsManageService.UpdateCcTerm(ccTerm, Operate.Update);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 保存叙词来源
        public JsonResult SaveSource(string source, string termGuid)
        {
            var ccTerm = _semanticsManageService.GetCcTermById(termGuid.ToInt32());
            if (ccTerm != null) ccTerm.Source = source;
            _semanticsManageService.UpdateCcTerm(ccTerm, Operate.Update);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 添加叙词来源
        public JsonResult AddTermSource(string termSourceModel, string text)
        {
            var newModel = termSourceModel.JsonTo<TermSourceModel>();
            newModel.CCCode = _semanticsManageService.GetSRByCC(text).CCCode;
            newModel.CreateDate = DateTime.Now;
            _semanticsManageService.UpdateTermSource(newModel, Operate.Create);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 根据数据来源名称删除来源
        public JsonResult DeleteBySourceName(string source)
        {
            _semanticsManageService.DeleteSourceByName(source);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 添加别名
        public JsonResult AddAlias(string semanticsModel)
        {
            var model = semanticsModel.JsonTo<SemanticsModel>();
            _semanticsManageService.AddAlias(model);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 删除别名
        public JsonResult DeleteAlias(string semanticsModel)
        {
            var model = semanticsModel.JsonTo<SemanticsModel>();
            _semanticsManageService.DeleteAlias(model);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 添加翻译词
        public JsonResult SaveTranslation(string model, string Translation)
        {
            var newmodel = model.JsonTo<TermTranslationModel>();
            newmodel.CreatedDate = newmodel.LastUpdatedDate = DateTime.Now;
            newmodel.CreatedBy = newmodel.LastUpdatedBy = CurrentUser.Name;
            _semanticsManageService.UpdateTermTranslation(newmodel, Operate.Create);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 删除翻译词
        public JsonResult DeleteTranslation(string tuid, string tran)
        {
            _semanticsManageService.DeleteTranslation(tuid.ToInt32(), tran);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // 排序
        public JsonResult RefreshResult(string nodeId, string parentName, int newOrderIndex)
        {
            _semanticsManageService.SequenceNodeEf(nodeId.ToInt32(), parentName, newOrderIndex);
            _semanticsManageService.UpdatePathTerm(nodeId.ToInt32());
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 语义关系类型维护

        //语义关系类型维护
        public ActionResult SemanticsType()
        {
            return View();
        }

        public JsonResult GetSemanticsTypeList()
        {
            var list = _semanticsManageService.GetSemanticsTypeList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SemanticsTypeAdd()
        {
            return View();
        }

        public ActionResult SemanticsTypeEdit(string model)
        {
            if (model == null) return null;
            var seModel = model.JsonTo<SemanticsTypeItem>();
            ViewData["model"] = seModel;
            return View();
        }

        public JsonResult UpdateSemanticsType(string model, string state)
        {
            var data = new Dictionary<string, string>();
            var newSemanticsModel = model.JsonTo<SemanticsTypeItem>();
            newSemanticsModel.LastUpdatedDate = DateTime.Now;
            newSemanticsModel.LastUpdatedBy = CurrentUser.Name;
            var operate = state.ToEnum<Operate>();
            try
            {
                _semanticsManageService.UpdateSemanticsType(newSemanticsModel, operate);
                data.Add("State", "success");
                if (operate == Operate.Create) data.Add("Text", "数据添加成功！");
                if (operate == Operate.Update) data.Add("Text", "数据修改成功！");
                if (operate == Operate.Delete) data.Add("Text", "数据删除成功！");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Add("State", "error");
                data.Add("Text", e.Message);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 语义关系维护

        //语义关系维护
        public ActionResult SemanticsRelation()
        {
            return View();
        }

        // 根据叙词和正向语义关系来获得对应概念类的叙词列表
        public JsonResult GetSemantics(string term, string sr)
        {
            return Json(_semanticsManageService.GetSemantics(term, sr, Direction.Forward), JsonRequestBehavior.AllowGet);
        }

        // 根据叙词和反向语义关系来获得对应概念类的叙词列表
        public JsonResult GetReverseSemantics(string term, string sr)
        {
            return Json(_semanticsManageService.GetSemantics(term, sr, Direction.Reverse), JsonRequestBehavior.AllowGet);
        }

        // 根据概念类code获得树
        public ActionResult CCTrees(string cc, string term, string sr, string id)
        {
            ViewBag.cc = cc;
            ViewBag.sr = sr;
            ViewBag.term = term;
            ViewBag.selectdId = id;
            return View();
        }

        // 获得需要维护的概念类的列表
        public JsonResult GetNeedManageCc()
        {
            var dics = _semanticsManageService.GetNeedManageCC();
            return Json(dics.Select(t => new { Id = t.Key, Text = t.Value }), JsonRequestBehavior.AllowGet);
        }

        // 根据概念类ccode获得所有叙词并以树结构展示
        public JsonResult GetTermTrees(string cc)
        {
            var ccTermInfo = _semanticsManageService.GetSRByCccode(cc);
            return Json(_semanticsManageService.GetCcTermTree(cc, ccTermInfo.SR), JsonRequestBehavior.AllowGet);
        }

        // 获得指定概念类需要维护的关系列表
        public JsonResult GetRelationOfCc(string cc)
        {
            var results = _semanticsManageService.GetRelationOfCC(cc);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // 添加新的语义关系到语义表中（重复的不再插入）
        public void SaveSemantics(string s)
        {
            _semanticsManageService.SaveSemantics(JsonToSemantics(s));
        }

        // 删除语义关系
        public void DeleteSemantics(string s)
        {
            _semanticsManageService.DeleteSemantics(JsonToSemantics(s));
        }

        private List<SemanticsModel> JsonToSemantics(string json)
        {
            var semantics = new List<SemanticsModel>();
            var userName = CurrentUser.Name;
            var words = Server.UrlDecode(json);
            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                var i = 0;
                foreach (var token in tokens)
                {
                    var fTermClassId = token["FTermClassId"].ToString();
                    var sr = token["SR"].ToString();
                    var lTermClassId = token["LTermClassId"].ToString();
                    var fTerm = token["FTerm"].ToString();
                    var lTerm = token["LTerm"].ToString();
                    var model = new SemanticsModel()
                    {
                        FTermClassId = fTermClassId.ToInt32(),
                        LTermClassId = lTermClassId.ToInt32(),
                        FTerm = fTerm,
                        LTerm = lTerm,
                        SR = sr,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = userName,
                        LastUpdatedDate = DateTime.Now,
                        OrderIndex = i++
                    };
                    semantics.Add(model);
                }
            }
            return semantics;
        }
        #endregion

        #region 成果类型上下文维护

        // 成果类型上下文维护
        public ActionResult PtContext()
        {
            return View();
        }

        /// <summary>
        /// 概念类树弹出窗口
        /// </summary>
        /// <param name="cc">选中单元格所属的概念类</param>
        /// <param name="sr">选中单元格和成果类型的关系类型</param>
        /// <param name="ptid">选中单元格对应的成果类型id</param>
        /// <returns></returns>
        public ActionResult TermTree(string cc, string sr, string ptid)
        {
            ViewBag.cc = cc;
            ViewBag.sr = sr.Trim();
            ViewBag.ptid = ptid;
            return View();
        }

        // 获得与PT有关系的关系类型
        public JsonResult GetPtRelations()
        {
            var relations = _semanticsManageService.GetPtRelations();
            return Json(relations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPtRelationsOfCc(string ptid, string sr)
        {
            var relations = _semanticsManageService.GetPtRelationsOfCc(ptid, sr);
            return Json(relations, JsonRequestBehavior.AllowGet);
        }

        public void DeletePtSemantics(string ptid, string sr, string term)
        {
            _semanticsManageService.DeletePtSemantics(ptid.ToInt32(), sr, term);
        }

        // 获得不同概念类树
        public JsonResult GetPTTermTree(string cc)
        {
            var data = _semanticsManageService.GetPtTermTree(cc)
                .Select(t => new
                {
                    id = t.TermClassId,
                    text = t.Term,
                    pid = t.PId
                });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // 获得pt上下文中某种概念类叙词列表（用于过滤）
        public JsonResult GetTermGroup(string pt, string field)
        {
            //这里需要加成果类型的过滤
            var result = new List<ModelBase>();
            if (!string.IsNullOrEmpty(field))
            {
                var data = _semanticsManageService.GetTermGroup(pt, field);
                var i = 0;
                var enumerable = data as object[] ?? data.Cast<object>().ToArray();
                foreach (var o in enumerable)
                {
                    var term = JObject.Parse(o.ToJson());
                    var id = i++.ToString(CultureInfo.InvariantCulture);
                    var text = term[field] ?? "空白";
                    result.AddRange(new[] { new ModelBase() { id = id, text = text.ToString() } });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // 单列 过滤pt上下文
        public JsonResult FilterPtContext(string pt, string filterItems, string field)
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            var data = _semanticsManageService.FilterPtContext(pt, filterItems, field, pageIndex, pageSize);
            var totalCount = _semanticsManageService.GetFilterPtContextCount(pt, filterItems, field);

            var hs = new Hashtable();
            hs["data"] = data;
            hs["total"] = totalCount;
            return Json(hs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///保存修改的PT上下文关系
        /// </summary>
        /// <param name="ptId">修改对应的PTID</param>
        /// <param name="pt">修改对应的pt</param>
        /// <param name="sr">修改的语义关系类型</param>
        /// <param name="ccId">叙词id</param>
        /// <param name="ccTerm">叙词</param>
        /// <returns></returns>
        public void SavePtContext(string ptId, string pt, string sr, string ccId, string ccTerm)
        {
            var userName = CurrentUser.Name;
            _semanticsManageService.SavePtContext(ptId, pt, sr, ccId, ccTerm, userName);
        }

        #endregion
    }
}