using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Semantic.Model;
using PKS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    /// <summary> 语义管理服务 </summary>
    public class SemanticsManageService : AppService, IPerRequestAppService
    {
        private readonly IRepository<SD_ConceptClass> _conceptClassRepository;
        private readonly IRepository<SD_SemanticsType> _semanticsTypeRepository;
        private readonly IRepository<SD_CCTerm> _ccTermRepository;
        private readonly IRepository<SD_Semantics> _semanticsRepository;
        private readonly IRepository<SD_TermSource> _termSourceRepository;
        private readonly IRepository<SD_TermTranslation> _termTranslationRepository;
        private readonly IRepository<SD_TermKeyword> _termKeywordrepository;
        private readonly IRepository<SMT_PTContextView> _ptContextrepository;

        public SemanticsManageService(IRepository<SD_ConceptClass> conceptClassRepository,
                                      IRepository<SD_SemanticsType> semanticsTypeRepository,
                                      IRepository<SD_CCTerm> ccTermRepository,
                                      IRepository<SD_Semantics> semanticsRepository,
                                      IRepository<SD_TermSource> termSourceRepository,
                                      IRepository<SD_TermTranslation> termTranslationRepository,
                                      IRepository<SD_TermKeyword> termKeywordrepository,
                                      IRepository<SMT_PTContextView> ptContextrepository)
        {
            _conceptClassRepository = conceptClassRepository;
            _semanticsTypeRepository = semanticsTypeRepository;
            _ccTermRepository = ccTermRepository;
            _semanticsRepository = semanticsRepository;
            _termSourceRepository = termSourceRepository;
            _termTranslationRepository = termTranslationRepository;
            _termKeywordrepository = termKeywordrepository;
            _ptContextrepository = ptContextrepository;
        }

        #region 维护概念类

        public List<ConceptClassItem> GetConceptClassList()
        {
            var query = _conceptClassRepository.GetQuery()
                .Where(w => w.CC != null && w.CCCode != "BS")
                .ToList();
            var result = query.Select(t => new ConceptClassItem
            {
                CCCode = t.CCCode,
                CC = t.CC,
                Type = t.Type,
                Source = t.Source,
                Remark = t.Remark
            }).ToList();
            return result;
        }

        public void UpdateConceptClass(ConceptClassItem item, Operate operate)
        {
            var entity = new SD_ConceptClass
            {
                CCCode = item.CCCode,
                CC = item.CC,
                Type = item.Type,
                Source = item.Source,
                Remark = item.Remark
            };

            

            switch (operate)
            {
                case Operate.Create:
                    _conceptClassRepository.Add(entity);
                    var srentity = new SD_SemanticsType
                    {
                        SR = "R_" + entity.CCCode + "_XF_" + entity.CCCode,
                        CCCode1 = entity.CCCode,
                        CCCode2 = entity.CCCode,
                        Description = entity.CC + "下分" + entity.CC,
                        CreatedBy = "system",
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = "system",
                        LastUpdatedDate = DateTime.Now,
                        Remark = "系统添加"
                    };
                    _semanticsTypeRepository.Add(srentity);
                    break;
                case Operate.Delete:
                    var conceptClassItem = _conceptClassRepository.GetQuery()
                        .Where(t => t.CCCode == entity.CCCode).FirstOrDefault();
                    if (conceptClassItem != null) _conceptClassRepository.Delete(conceptClassItem);
                    break;
                default:
                    _conceptClassRepository.Update(entity);
                    break;
            }
        }

        #endregion

        #region 维护语义关系类型

        public List<SemanticsTypeItem> GetSemanticsTypeList()
        {
            var query = _semanticsTypeRepository.GetQuery().ToList();
            var result = query.Select(t => new SemanticsTypeItem
            {
                SR = t.SR,
                CCCode1 = t.CCCode1,
                CCCode2 = t.CCCode2,
                Description = t.Description,
                CreatedBy = t.CreatedBy,
                CreatedDate = t.CreatedDate,
                LastUpdatedBy = t.LastUpdatedBy,
                LastUpdatedDate = t.LastUpdatedDate,
                Remark = t.Remark
            }).ToList();
            return result;
        }

        public List<PtRelations> GetPtRelations()
        {
            var result = new List<PtRelations>();
            var relations = _semanticsTypeRepository.GetQuery()
                .Where(t => t.SR.Contains("PT"))
                .ToList();
            foreach (var r in relations)
            {
                var field = r.CCCode1 != "PT" ? r.CCCode1 : r.CCCode2;
                if (r.SR.Contains("SY")) field = "U" + field;
                result.Add(new PtRelations() { SR = r.SR, Field = field });
            }
            return result;
        }

        public object GetPtRelationsOfCc(string ptid, string sr)
        {
            var index = sr.IndexOf("PT", StringComparison.Ordinal);
            //反向
            if (index > 3) return GetSemantics(ptid.ToInt32(), sr, Direction.Reverse)
                    .Select(s => new { id = s.FTermClassId, text = s.FTerm })
                    .ToList();
            //正向
            return GetSemantics(ptid.ToInt32(), sr, Direction.Forward)
                .Select(s => new { id = s.LTermClassId, text = s.LTerm })
                .ToList();
        }

        public void UpdateSemanticsType(SemanticsTypeItem item, Operate operate)
        {
            var entity = new SD_SemanticsType
            {
                SR = item.SR,
                CCCode1 = item.CCCode1,
                CCCode2 = item.CCCode2,
                Description = item.Description,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                Remark = item.Remark
            };

            switch (operate)
            {
                case Operate.Create:
                    entity.CreatedBy = entity.LastUpdatedBy;
                    entity.CreatedDate = entity.LastUpdatedDate;
                    _semanticsTypeRepository.Add(entity);
                    break;
                case Operate.Delete:
                    var semanticsTypeItem = _semanticsTypeRepository.GetQuery()
                        .Where(t => t.SR == entity.SR).FirstOrDefault();
                    if (semanticsTypeItem != null) _semanticsTypeRepository.Delete(semanticsTypeItem);
                    break;
                default:
                    _semanticsTypeRepository.Update(entity);
                    break;
            }
        }

        #endregion

        #region 维护叙词 

        public List<TermTreeModel> GetCcTermTree(string cccode, string sr = null)
        {         
            var termTree = _ccTermRepository.GetQuery()
                .Where(t => t.CCCode == cccode)
                .Select(t => new TermTreeModel
                {
                    TermClassId = t.TermClassID,
                    Term = t.Term,
                    PId = null,
                    lvl = 1,
                    Source = t.Source,
                    OrderIndex = t.OrderIndex,
                    IsLeaf = 1,
                    isChecked = false,
                    PathTerm = t.PathTerm
                })
                .OrderBy(t => t.OrderIndex)
                .ToList();

            var semantics = _semanticsRepository.GetQuery()
                .Where(t => t.SR == sr);

            foreach (var semantic in semantics)
            {
                var termTreeItem = termTree.Where(t => t.TermClassId == semantic.LTermClassId)
                    .FirstOrDefault();
                if (termTreeItem != null)
                {
                    termTreeItem.PId = semantic.FTermClassId;
                    termTreeItem.lvl += 1;
                }
            }
            return termTree;
        }

        public void EditCcTermTreeById(int termGuid, string cctermName, string path)
        {
            var ccTerm = _ccTermRepository.GetQuery().FirstOrDefault(t => t.TermClassID == termGuid);
            if (ccTerm == null) throw new ArgumentNullException("不存在该值！");
            ccTerm.Term = cctermName;
            var fSematics = _semanticsRepository.GetQuery().Where(o => o.FTermClassId == termGuid);//在语义表中找到实体
            var lSematics = _semanticsRepository.GetQuery().Where(o => o.LTermClassId == termGuid);
            if (fSematics.Count() >= 0 && lSematics.Count() == 0)//找到的根节点含有子节点
            {
                ccTerm.PathTerm = cctermName;
                foreach(var item in fSematics)
                    item.FTerm = cctermName;//修改根节点下子节点的首词
            }
            if (lSematics.Count() > 0)//找到的节点不是根节点
            {
                ccTerm.PathTerm = path + "/" + cctermName;
                foreach(var item in lSematics)
                    item.LTerm = cctermName;
                if (fSematics.Count() > 0)//找到的节点是含有子节点的节点
                {
                    foreach (var item in fSematics)
                        item.FTerm = cctermName;//修改含有子节点的节点的名称
                }
            }
            _ccTermRepository.Submit();
            _semanticsRepository.Submit();
        }

        public CcTermModel GetCcTermById(int termClassId)
        {
            var ccTerm = _ccTermRepository.GetQuery()
                .Where(t => t.TermClassID == termClassId).FirstOrDefault();
            if (ccTerm == null) return null;
            return new CcTermModel
            {
                TermClassID = ccTerm.TermClassID,
                CCCode = ccTerm.CCCode,
                Term = ccTerm.Term,
                PathTerm = ccTerm.PathTerm,
                Source = ccTerm.Source,
                Description = ccTerm.Description,
                OrderIndex = ccTerm.OrderIndex,
                CreatedDate = ccTerm.CreatedDate,
                CreatedBy = ccTerm.CreatedBy,
                LastUpdatedDate = ccTerm.LastUpdatedDate,
                LastUpdatedBy = ccTerm.LastUpdatedBy,
                Remark = ccTerm.Remark
            };
        }

        public void DeleteCcTermById(int termClassId)
        {
            _termKeywordrepository.DeleteList(t => t.TermClassID == termClassId);
            _termTranslationRepository.DeleteList(t => t.TermClassID == termClassId);
            _semanticsRepository.DeleteList(t => t.FTermClassId == termClassId || t.LTermClassId == termClassId);
            _ccTermRepository.DeleteList(t => t.TermClassID == termClassId);
        }

        public int UpdateCcTerm(CcTermModel item, Operate operate)
        {
            var entity = new SD_CCTerm
            {
                CCCode = item.CCCode,
                Term = item.Term,
                LangCode = null,
                PathTerm = item.PathTerm,
                Source = item.Source,
                Description = item.Description,
                OrderIndex = item.OrderIndex,
                CreatedDate = item.CreatedDate,
                CreatedBy = item.CreatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                Remark = item.Remark
            };

            if (operate != Operate.Create)
            {
                entity.TermClassID = item.TermClassID;
            }

            switch (operate)
            {
                case Operate.Create:
                    _ccTermRepository.Add(entity);
                    break;
                case Operate.Delete:
                    var ccTermItem = _ccTermRepository.GetQuery()
                        .Where(t => t.TermClassID == entity.TermClassID).FirstOrDefault();
                    if (ccTermItem != null) _ccTermRepository.Delete(ccTermItem);
                    break;
                default:
                    _ccTermRepository.Update(entity);
                    break;
            }
            // TODU:
            return entity.TermClassID;
        }

        /// <summary>
        /// 拖拽节点
        /// 1、删除和以前和父节点的关系
        /// 2、建立和新的父节点的关系
        /// 3、更新叙词的关系
        /// </summary>
        /// <param name="sdModel">语义关系实体</param>
        /// <param name="dragmodel">拖拽节点实体</param>
        /// <param name="strSr">概念类名称</param>
        public void DragNodeEf(SemanticsModel sdModel, CcTermModel dragmodel, string cc)
        {
            var sr = GetSRByCC(cc).SR;
            if (string.IsNullOrEmpty(sr)) return;
            //判断拖拽的节点是不是根节点
            //countSemantics == 0,是根节点
            var countSemantics = _semanticsRepository.Count(o => o.LTermClassId == dragmodel.TermClassID && o.SR == sr);
            //当拖拽的是根节点是
            if (countSemantics == 0 && sdModel.FTerm != null)
            {
                sdModel.SR = sr;  //修改关系
                UpdateSemantics(sdModel, Operate.Create);
            }
            //拖拽的不是根节点
            else
            {
                var oldsdmodel = _semanticsRepository.GetQuery().FirstOrDefault(o => o.LTermClassId == dragmodel.TermClassID && o.SR == sr);
                //投放的位置是根节点
                if (sdModel.FTerm == null)
                {
                    _semanticsRepository.Delete(oldsdmodel);
                }
                //投放的位置不是根节点
                else
                {
                    _semanticsRepository.Delete(oldsdmodel);
                    sdModel.SR = sr;
                }
            }
            _semanticsRepository.Submit();
        }

        /// <summary>
        /// 节点排序
        /// 1、给根节点排序:获得所有的根节点，进行排序
        /// 2、给根节点下的子节点排序：获得节点下的子节点，进行排序
        /// </summary>
        /// <param name="nodeId">叙词id</param>
        /// <param name="parentName">父节点的名称</param>
        /// <param name="newOrderIndex">节点新的序号</param>
        /// <param name="parentPath">节点新的路径</param>
        public void SequenceNodeEf(int nodeId, string parentName, int newOrderIndex)
        {
            var sdSematics = _semanticsRepository.GetQuery().FirstOrDefault(o => o.LTermClassId == nodeId);
            var sdCcterm = _ccTermRepository.GetQuery().FirstOrDefault(c => c.TermClassID == nodeId);
            //找到的节点是根节点
            if (sdSematics == null)
            {
                if (sdCcterm != null)
                {
                    sdCcterm.OrderIndex = newOrderIndex;
                }
            }
            else
            {
                if (sdCcterm != null && parentName != null)
                {
                    sdSematics.FTerm = parentName;
                    sdSematics.LTerm = sdCcterm.Term;
                    sdSematics.OrderIndex = newOrderIndex;
                    _semanticsRepository.Update(sdSematics);
                    sdCcterm.OrderIndex = newOrderIndex;
                    _ccTermRepository.Update(sdCcterm);
                }
            }
            _semanticsRepository.Submit();
        }

        public void UpdatePathTerm(int termClassId)
        {
            var ccTerm = _ccTermRepository.GetQuery()
                .Where(t => t.TermClassID == termClassId)
                .FirstOrDefault();
            if (ccTerm == null) return;
            ccTerm.PathTerm = GetPath(termClassId);
            _ccTermRepository.Submit();
        }

        public string GetPath(int termClassId)
        {
            int CurrentTermClassId = termClassId;
            int? ParentTermClassId = termClassId;
            var ReturnText = string.Empty;
            var ccTerm = _ccTermRepository.GetQuery().FirstOrDefault(t => t.TermClassID == termClassId);
            if (ccTerm != null) ReturnText = ccTerm.Term;
            while (ParentTermClassId != null)
            {
                ParentTermClassId = null;
                var query = from s in _semanticsRepository.GetQuery()
                            join cc1 in _ccTermRepository.GetQuery() on s.FTermClassId equals cc1.TermClassID into cc1_join
                            from x in cc1_join.DefaultIfEmpty()
                            join cc2 in _ccTermRepository.GetQuery() on s.LTermClassId equals cc2.TermClassID into cc2_join
                            from v in cc2_join.DefaultIfEmpty()
                            where s.LTermClassId == CurrentTermClassId && x.CCCode == v.CCCode && s.SR.Contains("XF")
                            select new
                            {
                                id = s.FTermClassId
                            };

                ParentTermClassId = query.ToList().FirstOrDefault().id;
                if (ParentTermClassId != null)
                {
                    var newCcTerm = _ccTermRepository.GetQuery()
                        .Where(t => t.TermClassID == ParentTermClassId).FirstOrDefault();
                    if (newCcTerm != null)
                    {
                        CurrentTermClassId = ParentTermClassId.Value;
                        ReturnText = newCcTerm.Term + '/' + ReturnText;
                    }
                }
            }
            return ReturnText;
        }

        #endregion

        #region 维护语义关系

        public List<CcTermModel> GetSemantics(string term, string sr, Direction direction)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(sr))
                throw new ArgumentNullException("term or sr");
            List<int> termIds = null;
            if (direction == Direction.Forward)
            {
                 termIds = _semanticsRepository.GetQuery()
                    .Where(t => t.FTerm == term && t.SR == sr)
                    .Select(t => t.LTermClassId)
                    .ToList();            
            }
            if(direction == Direction.Reverse)
            {
                termIds = _semanticsRepository.GetQuery()
                    .Where(t => t.LTerm == term && t.SR == sr)
                    .Select(t => t.FTermClassId)
                    .ToList();
            }
            if (termIds.Count != 0)
                return _ccTermRepository.GetQuery()
                .Where(t => termIds.Contains(t.TermClassID))
                .Select(t => new CcTermModel
                {
                    TermClassID = t.TermClassID,
                    CCCode = t.CCCode,
                    Term = t.Term,
                    PathTerm = t.PathTerm,
                    Source = t.Source,
                    Description = t.Description,
                    OrderIndex = t.OrderIndex,
                    CreatedDate = t.CreatedDate,
                    CreatedBy = t.CreatedBy,
                    LastUpdatedDate = t.LastUpdatedDate,
                    LastUpdatedBy = t.LastUpdatedBy,
                    Remark = t.Remark
                }).ToList();
            return new List<CcTermModel>();
        }

        public void SaveSemantics(List<SemanticsModel> semantics)
        {
            foreach (var semantic in semantics)
            {
                var oldSemantics = _semanticsRepository.GetQuery()
                    .FirstOrDefault(a =>
                    a.FTermClassId.Equals(semantic.FTermClassId) &&
                    a.SR.Equals(semantic.SR) &&
                    a.LTermClassId.Equals(semantic.LTermClassId));
                //如果要添加的数据不存在 添加
                if (oldSemantics == null) UpdateSemantics(semantic, Operate.Create);
                //如果数据存在，判断是否更新orderindex。变更则更新，没变更则不操作
                else
                {
                    //对原来的数据进行重新赋值
                    if (oldSemantics.OrderIndex != semantic.OrderIndex)
                    {
                        oldSemantics.OrderIndex = semantic.OrderIndex;
                        oldSemantics.LastUpdatedBy = semantic.LastUpdatedBy;
                        oldSemantics.LastUpdatedDate = DateTime.Now;
                        _semanticsRepository.Submit();
                    }
                }
            }
        }

        public void DeleteSemantics(List<SemanticsModel> semantics)
        {
            foreach (var semantic in semantics)
            {
                var oldSemantics = _semanticsRepository.GetQuery().FirstOrDefault(a =>
                               a.FTermClassId.Equals(semantic.FTermClassId) && a.SR.Equals(semantic.SR) &&
                               a.LTerm.Equals(semantic.LTerm));
                if (oldSemantics!=null)_semanticsRepository.Delete(oldSemantics);
            }
        }

        public void DeletePtSemantics(int ptid, string sr, string term)
        {
            var oldSemantics = _semanticsRepository.GetQuery()
                .Where(a => (a.FTermClassId == ptid || a.LTermClassId == ptid) &&
                (a.FTerm.Equals(term) || a.LTerm.Equals(term)) && 
                a.SR.Equals(sr));
            if (oldSemantics != null) _semanticsRepository.DeleteList(oldSemantics);
        }

        public List<TermTreeModel> GetPtTermTree(string cc)
        {
            if (cc.Equals("BS"))
                return _ccTermRepository.GetQuery()
                    .Where(t => t.CCCode.Equals(cc))
                    .Select(t => new TermTreeModel
                    {
                        TermClassId = t.TermClassID,
                        Term = t.Term
                    }).ToList();
            var termInfo = GetSRByCccode(cc);
            return GetCcTermTree(termInfo.CCCode, termInfo.SR);
        }

        public IEnumerable GetTermGroup(string pt, string field)
        {
            if (field == null) throw new ArgumentNullException("field");
            var returnField = string.Format("new ({0})", field);
            return _ptContextrepository.GetQuery()
                .Where(w => w.PT.Contains(pt))
                .Select(returnField)
                .Distinct();
        }

        //根据概念类名称获取CCCode和语义关系
        public CcTermInfo GetSRByCC(string cc)
        {
            string sr = string.Empty;
            if (string.IsNullOrEmpty(cc)) cc = "业务过程";
            string cccode = _conceptClassRepository.GetQuery()
                    .FirstOrDefault(t => t.CC == cc)?.CCCode;
            if (string.IsNullOrEmpty(cccode)) return null;
            var query = _semanticsTypeRepository.GetQuery()
                      .FirstOrDefault(t => t.CCCode1 == cccode && t.CCCode2 == cccode && t.SR.Contains("XF"));
            if (query != null) sr = query.SR;
            return new CcTermInfo { CCCode = cccode, SR = sr };
        }

        public CcTermInfo GetSRByCccode(string cccode)
        {
            string sr = string.Empty;
            if (cccode == "PT") sr = "R_BP_CS_PT";
            else
            {
                var query = _semanticsTypeRepository.GetQuery()
                      .FirstOrDefault(t => t.CCCode1 == cccode && t.CCCode2 == cccode && t.SR.Contains("XF"));
                if (query != null) sr = query.SR;
            }
            return new CcTermInfo { CCCode = cccode, SR = sr };
        }

        public List<SemanticsModel> GetSemantics(int id, string sr, Direction direction)
        {
            if (string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"sr");
            IQueryable<SD_Semantics> query = null;
            if (direction == Direction.Forward)
                query = _semanticsRepository.GetQuery()
                   .Where(t => t.FTermClassId == id && t.SR.Equals(sr.Trim()));
            if(direction == Direction.Reverse)
                query = _semanticsRepository.GetQuery()
                   .Where(t => t.LTermClassId == id && t.SR.Equals(sr.Trim()));
            return query
                .Select(t => new SemanticsModel
                {
                    FTermClassId = t.FTermClassId,
                    SR = t.SR,
                    LTermClassId = t.LTermClassId,
                    FTerm = t.FTerm,
                    LTerm = t.LTerm,
                    OrderIndex = t.OrderIndex,
                    CreatedDate = t.CreatedDate,
                    CreatedBy = t.CreatedBy,
                    LastUpdatedDate = t.LastUpdatedDate,
                    LastUpdatedBy = t.LastUpdatedBy,
                    Remark = t.Remark,
                    IsLeaf = false
                })
                .ToList();
        }

        public void UpdateSemantics(SemanticsModel item, Operate operate)
        {
            var entity = new SD_Semantics
            {
                FTermClassId = item.FTermClassId,
                SR = item.SR,
                LTermClassId = item.LTermClassId,
                FTerm = item.FTerm,
                LTerm = item.LTerm,
                OrderIndex = item.OrderIndex,
                CreatedDate = item.CreatedDate,
                CreatedBy = item.CreatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                Remark = item.Remark
            };

            switch (operate)
            {
                case Operate.Create:
                    _semanticsRepository.Add(entity);
                    break;
                case Operate.Delete:
                    var semanticsItem = _semanticsRepository.GetQuery()
                        .Where(t => t.FTermClassId == entity.FTermClassId && t.LTermClassId == entity.LTermClassId && t.SR == entity.SR)
                        .FirstOrDefault();
                    if (semanticsItem != null) _semanticsRepository.Delete(semanticsItem);
                    break;
                default:
                    _semanticsRepository.Update(entity);
                    break;
            }
        }

        // 获得需要维护的概念类
        public Dictionary<string, string> GetNeedManageCC()
        {
            var results = new Dictionary<string, string>();
            var cccodes = _semanticsTypeRepository.GetQuery()
                .Where(w => !w.SR.Contains("XF"))
                .Select(s => s.CCCode1).Distinct().ToList();
            foreach (var cccode in cccodes)
            {
                if (cccode != null)
                {
                    results.Add(cccode, _conceptClassRepository.GetQuery()
                        .Where(w => w.CCCode.Equals(cccode))
                        .Select(s => s.CC).FirstOrDefault()
                        );
                }
            }
            return results;
        }

        // 获得指定概念类需要维护的关系类型列表
        public List<SemanticsTypeItem> GetRelationOfCC(string cc)
        {
            cc = cc.Trim(new[] { '\'' });

            var lists = _semanticsTypeRepository.GetQuery()
                .Where(w => !w.SR.Contains("XF"))
                .Where(w => w.CCCode1.Equals(cc))
                .Distinct()
                .Select(t => new SemanticsTypeItem
                {
                    SR = t.SR,
                    CCCode1 = t.CCCode1,
                    CCCode2 = t.CCCode2,
                    Description = t.Description,
                    CreatedBy = t.CreatedBy,
                    CreatedDate = t.CreatedDate,
                    LastUpdatedBy = t.LastUpdatedBy,
                    LastUpdatedDate = t.LastUpdatedDate,
                    Remark = t.Remark
                })
                .ToList();
            return lists;
        }

        #endregion

        #region 维护翻译词

        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="semantics">语义关系模型</param>
        public void AddAlias(SemanticsModel semantics)
        {
            var cc = GetCcTermById(semantics.FTermClassId).CCCode;

            //先查看添加的别名在数据库中是否存在
            var newTermClassId = _ccTermRepository.GetQuery()
                .Where(t => t.Term.Equals(semantics.LTerm) && t.CCCode.Equals(cc))
                .Select(t => t.TermClassID)
                .FirstOrDefault();

            //若存在只需添加代的关系
            if (newTermClassId != 0)
            {
                semantics.LTermClassId = newTermClassId;
            }
            //若不存在，需将新词添加到叙词表中 再添加代的关系
            else
            {              
                var newTerm = new CcTermModel()
                {
                    CCCode = cc,
                    Term = semantics.LTerm,
                    TermClassID = semantics.LTermClassId,
                    CreatedBy = semantics.CreatedBy,
                    CreatedDate = semantics.CreatedDate,
                    LastUpdatedBy = semantics.LastUpdatedBy,
                    OrderIndex = semantics.OrderIndex
                };
                //添加的别名 添加到分类叙词表中 别名的概念类与原词相同
                newTerm.OrderIndex = _ccTermRepository.GetQuery().Where(t => t.CCCode == cc).Max(t => t.OrderIndex) + 1;                
                semantics.LTermClassId = UpdateCcTerm(newTerm, Operate.Create);
            }
            //添加‘D’的关系到语义关系表中
            UpdateSemantics(semantics, Operate.Create);
        }
        /// <summary>
        /// 删除别名
        /// </summary>
        /// <param name="semantics">语义关系模型</param>
        public void DeleteAlias(SemanticsModel semantics)
        {
            //只需要删除该“D”的关系，不删除叙词分类表中信息
            UpdateSemantics(semantics, Operate.Delete);
        }

        /// <summary>
        /// 获取翻译词
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TermTranslationModel> GetTermTraslations(int id)
        {
            return _termTranslationRepository.GetQuery()
                .Where(t => t.TermClassID == id)
                .Select(t => new TermTranslationModel
                {
                    TermClassID = t.TermClassID,
                    LangCode = t.LangCode,
                    Translation = t.Translation,
                    IsMain = t.IsMain,
                    OrderIndex = t.OrderIndex,
                    CreatedDate = t.CreatedDate,
                    CreatedBy = t.CreatedBy,
                    LastUpdatedDate = t.LastUpdatedDate,
                    LastUpdatedBy = t.LastUpdatedBy,
                    Remark = t.Remark
                })
                .ToList();
        }

        public void DeleteTranslation(int tuid, string tran)
        {
            _termTranslationRepository.DeleteList(t => t.TermClassID == tuid && t.Translation == tran);
        }

        public void UpdateTermTranslation(TermTranslationModel item, Operate operate)
        {
            var entity = new SD_TermTranslation
            {
                TermClassID = item.TermClassID,
                LangCode = item.LangCode,
                Translation = item.Translation,
                IsMain = item.IsMain,
                OrderIndex = item.OrderIndex,
                CreatedDate = item.CreatedDate,
                CreatedBy = item.CreatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                Remark = item.Remark
            };

            switch (operate)
            {
                case Operate.Create:
                    var count = _termTranslationRepository.GetQuery().Max(t => t.OrderIndex);
                    if (!count.HasValue) count = 0;
                    entity.OrderIndex = count + 1;
                    _termTranslationRepository.Add(entity);
                    break;
                case Operate.Delete:
                    var termTranslationItem = _termTranslationRepository.GetQuery()
                        .Where(t => t.TermClassID == entity.TermClassID && t.LangCode == entity.LangCode && t.Translation == entity.Translation)
                        .FirstOrDefault();
                    if (termTranslationItem != null) _termTranslationRepository.Delete(termTranslationItem);
                    break;
                default:
                    _termTranslationRepository.Update(entity);
                    break;
            }
        }

        #endregion

        #region 维护叙词来源

        /// <summary>
        /// 获取来源
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public List<TermSourceModel> GetSourceList(string cc)
        {
            var ccTermInfo = GetSRByCC(cc);
            return _termSourceRepository.GetQuery()
                .Where(t => t.CCCode == ccTermInfo.CCCode)
                .Distinct()
                .Select(t => new TermSourceModel
                {
                    CCCode = t.CCCode,
                    Source = t.Source,
                })
                .ToList();
        }

        public List<string> GetSourceListById(int cctermGuid)
        {
            var source = _ccTermRepository.GetQuery()
                .Single(t => t.TermClassID == cctermGuid)?.Source;
            if (string.IsNullOrEmpty(source)) return new List<string>();
            string[] sources = source.Split(new[] { ',', '、' });
            return _termSourceRepository.GetQuery()
                .Where(t => sources.Contains(t.Source))
                .Select(t => t.Source)
                .ToList();
        }

        public void DeleteSourceByName(string sourceName)
        {
            _termSourceRepository.DeleteList(t => t.Source == sourceName);
        }

        public void UpdateTermSource(TermSourceModel item, Operate operate)
        {
            var entity = new SD_TermSource
            {
                CCCode = item.CCCode,
                Source = item.Source,
                CreateDate = item.CreateDate
            };

            switch (operate)
            {
                case Operate.Create:
                    _termSourceRepository.Add(entity);
                    break;
                case Operate.Delete:
                    var termSourceItem = _termSourceRepository.GetQuery()
                        .Where(t => t.CCCode == entity.CCCode && t.Source == entity.Source).FirstOrDefault();
                    if (termSourceItem != null) _termSourceRepository.Delete(termSourceItem);
                    break;
                default:
                    _termSourceRepository.Update(entity);
                    break;
            }
        }
        #endregion

        #region 维护关键词

        public List<TermKeyWords> GetKeyWordsById(int id)
        {
            return _termKeywordrepository.GetQuery()
                .Where(t => t.TermClassID == id)
                .OrderBy(t => t.OrderIndex)
                .Select(t => new TermKeyWords
                {
                    KeyWord = t.Keyword,
                    OrderIndex = t.OrderIndex.Value
                    
                }).ToList();
        }

        public void UpdateKeyWords(int termClassId, string userName, Dictionary<string, int> keywordsAndOrder, Operate operate)
        {
            foreach(var keyword in keywordsAndOrder)
            {
                var entity = new SD_TermKeyword
                {
                    TermClassID = termClassId,
                    Keyword = keyword.Key,
                    OrderIndex = keyword.Value,
                    LastUpdatedBy = userName,
                    LastUpdatedDate = DateTime.Now
                };

                var termKeyword = _termKeywordrepository.GetQuery()
                    .FirstOrDefault(t => t.TermClassID == termClassId && t.Keyword == keyword.Key);
                if(termKeyword == null)
                {
                    if (operate == Operate.Create)
                    {
                        entity.CreatedBy = userName;
                        entity.CreatedDate = DateTime.Now;
                        _termKeywordrepository.Add(entity);
                    }
                }
                else
                {
                    if (operate == Operate.Update)
                    {
                        termKeyword.OrderIndex = keyword.Value;
                        termKeyword.LastUpdatedBy = userName;
                        termKeyword.LastUpdatedDate = DateTime.Now;
                        _termKeywordrepository.Submit();
                    }
                    if (operate == Operate.Delete) _termKeywordrepository.Delete(termKeyword);
                }           
            }
        }

        public List<BPAndPTTreeModel> GetBPPTTree(string id, bool show)
        {
            var termTree = _ccTermRepository.GetQuery()
                .Where(t => t.CCCode.ToUpper() == "BP" || t.CCCode.ToUpper() == "PT")
                .OrderBy(t => t.OrderIndex)
                .Select(t => new BPAndPTTreeModel
                {
                    TermClassId = t.TermClassID,
                    Term = t.Term,
                    PId = null,
                    OrderIndex = t.OrderIndex.Value,
                    IsPT = t.CCCode.ToUpper() == "PT" ? "Y" : "N",
                    kwCount = 0
                })
                .ToList();

            var semantics = _semanticsRepository.GetQuery()
                .Where(t => t.SR.ToUpper() == "R_BP_XF_BP" || t.SR.ToUpper() == "R_BP_CS_PT" || t.SR.ToUpper() == "R_PT_XF_PT");

            foreach(var semantic in semantics)
            {
                var termTreeItem = termTree.Where(t => t.TermClassId == semantic.LTermClassId)
                    .FirstOrDefault();
                if (termTreeItem != null)
                {
                    termTreeItem.PId = semantic.FTermClassId;
                }
            }

            var keywordsCount = _termKeywordrepository.GetQuery()
                .GroupBy(t => t.TermClassID)
                .Select(t => new
                {
                    id = t.Key,
                    count = t.Count()
                });

            foreach(var item in keywordsCount)
            {
                var termTreeItem = termTree.Where(t => t.TermClassId == item.id)
                    .FirstOrDefault();
                if (termTreeItem != null)
                {
                    termTreeItem.kwCount = item.count;
                }
            }
            return termTree;
        }

        #endregion

        #region 维护成果类型上下文

        // 获得pt上下文
        public List<SMT_PTContextView> FilterPtContext(string pt, string filterItems, string field, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(filterItems) || string.IsNullOrEmpty(field))
            {
                return GetPtContext(pt, pageIndex, pageSize);
            }
            var whereCondition = GetWhereExpression(filterItems, field);
            return FilterPtContext(pt, whereCondition, pageIndex, pageSize);
        }

        // 获得过滤后的PT上下文
        public List<SMT_PTContextView> FilterPtContext(string pt, Expression<Func<SMT_PTContextView, bool>> conditions, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;
            var take = pageSize;
            return _ptContextrepository.GetQuery()
                .Where(w => w.PT.Contains(pt)).Where(conditions)
                .OrderBy(o => o.PT)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        // 获得成果类型上下文
        public List<SMT_PTContextView> GetPtContext(string pt, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;
            var take = pageSize;
            return _ptContextrepository.GetQuery()
                .Where(w => w.PT.Contains(pt) || pt == "")
                .OrderBy(o => o.PT)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        // 使用表达式树获得动态拼接where条件
        private Expression<Func<SMT_PTContextView, bool>> GetWhereExpression(string filterItems, string field)
        {
            var items = filterItems.Replace("空白", string.Empty).Split(',');
            var filter = new List<Filter>();
            foreach (var item in items)
            {
                filter.Add(new Filter() { Operation = Op.Equals, PropertyName = field, Value = item });
            }
            var filters = new FilterCollection { filter };
            return LambdaExpressionBuilder.GetExpression<SMT_PTContextView>(filters);
        }

        // 获得pt上下文，可带过滤条件 的个数
        public int GetFilterPtContextCount(string pt, string filterItems, string field)
        {
            if (string.IsNullOrEmpty(filterItems) || string.IsNullOrEmpty(field))
            {
                return _ptContextrepository.GetQuery().Count(t => t.PT.Contains(pt) || pt == "");
            }
            var whereCondition = GetWhereExpression(filterItems, field);
            return _ptContextrepository.GetQuery()
                .Where(w => w.PT.Contains(pt))
                .Where(whereCondition)
                .Count();
        }

        public void SavePtContext(string ptId, string pt, string sr, string ccId, string ccTerm, string userName)
        {
            var semantics = new List<SD_Semantics>();
            sr = sr.Trim();
            var index = sr.IndexOf("PT", StringComparison.Ordinal);
            if (!string.IsNullOrEmpty(ccId) && !string.IsNullOrEmpty(ccTerm))
            {
                var ccids = ccId.Split('|');
                var ccTerms = ccTerm.Split(Convert.ToChar('|'));
                for (int i = 0; i < ccids.Length; i++)
                {
                    var fid = ptId;
                    var lid = ccids[i];
                    var fterm = pt;
                    var lterm = ccTerms[i];
                    if (index > 3) { fid = ccids[i]; lid = ptId; fterm = ccTerms[i]; lterm = pt; }
                    var semantic = new SD_Semantics()
                    {
                        FTermClassId = fid.ToInt32(),
                        LTermClassId = lid.ToInt32(),
                        FTerm = fterm,
                        LTerm = lterm,
                        SR = sr,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = userName,
                        LastUpdatedDate = DateTime.Now,
                        OrderIndex = i
                    };
                    _semanticsRepository.Add(semantic);
                }
            }
        }

        #endregion
    }
}
