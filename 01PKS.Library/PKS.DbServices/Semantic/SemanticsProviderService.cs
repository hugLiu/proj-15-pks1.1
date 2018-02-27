using Jurassic.PKS.Service.Semantics;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Semantic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    /// <summary> 提供语义服务 </summary>
    public class SemanticsProviderService : AppService, IPerRequestAppService
    {
        private readonly IRepository<SD_ConceptClass> _conceptClassRepository;
        private readonly IRepository<SD_SemanticsType> _semanticsTypeRepository;
        private readonly IRepository<SD_CCTerm> _ccTermRepository;
        private readonly IRepository<SD_Semantics> _semanticsRepository;
        private readonly IRepository<SD_TermTranslation> _termTranslationRepository;

        public SemanticsProviderService(IRepository<SD_ConceptClass> conceptClassRepository,
                                      IRepository<SD_SemanticsType> semanticsTypeRepository,
                                      IRepository<SD_CCTerm> ccTermRepository,
                                      IRepository<SD_Semantics> semanticsRepository,
                                      IRepository<SD_TermTranslation> termTranslationRepository
)
        {
            _conceptClassRepository = conceptClassRepository;
            _semanticsTypeRepository = semanticsTypeRepository;
            _ccTermRepository = ccTermRepository;
            _semanticsRepository = semanticsRepository;
            _termTranslationRepository = termTranslationRepository;
        }

        #region 获得指定概念类的叙词信息
        /// <summary>
        /// 获得指定概念类的 叙词信息
        /// </summary>
        /// <param name="term">正式叙词</param>
        /// <param name="cc">概念类（可为空）</param>
        /// <returns>给定叙词（满足指定概念类）的详细信息</returns>
        public async Task<List<SD_CCTerm>> GetTermInfo(string term, string cc)
        {
            if (term == null) throw new ArgumentNullException(nameof(term));

            //如果cc为空则返回该叙词的所有概念类信息
            return await _ccTermRepository.GetQuery()
                .Where(w => (w.Term.Equals(term) || w.PathTerm.Equals(term))
                && (w.CCCode.Equals(cc) || cc.Equals("") || cc.Equals(null)))
                .Include("SD_ConceptClass").ToListAsync();
        }
        #endregion

        #region 获得指定叙词ID的翻译词列表
        /// <summary>
        /// 根据叙词ID获取其翻译词
        /// </summary>
        /// <param name="id">叙词ID</param>
        /// <param name="langCode">语言类型</param>
        /// <param name="onlyMain">是否只包含主词</param>
        /// <returns>翻译叙词结果列表</returns>
        public async Task<List<string>> GetTranslationById(int id, string langCode, bool onlyMain)
        {
            var query = _termTranslationRepository.GetQuery()
                .Where(tt => tt.TermClassID == id);

            if (!string.IsNullOrEmpty(langCode))
            {
                query = query.Where(tt => tt.LangCode.Equals(langCode));
            }

            if (onlyMain)
            {
                query = query.Where(w => w.IsMain == 1);
            }
            var task = Task.Run(() => query.OrderBy(o => o.OrderIndex).Select(s => s.Translation).Distinct().ToList());
            return await task;
        }

        /// <summary>
        /// 根据叙词获取其翻译词
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="langCode">语言类型</param>
        /// <param name="onlyMain">是否只包含主词</param>
        /// <returns>翻译叙词结果列表</returns>
        public async Task<List<string>> GetTranslationByName(string term, string langCode, bool onlyMain)
        {
            var query = from cc in _ccTermRepository.GetQuery()
                        join tt in _termTranslationRepository.GetQuery()
                        on cc.TermClassID equals tt.TermClassID
                        where cc.Term.Equals(term)
                        select tt;

            if (!string.IsNullOrEmpty(langCode))
            {
                query = query.Where(tt => tt.LangCode.Equals(langCode));
            }

            if (onlyMain)
            {
                query = query.Where(w => w.IsMain == 1);
            }

            return await query.Select(t => t.Translation).Distinct().ToListAsync();
        }

        #endregion

        #region 获得所有的概念类
        public async Task<List<SD_ConceptClass>> GetCC()
        {
            var task = Task.Run(() => _conceptClassRepository.GetQuery().ToList());
            return await task;
        }
        #endregion

        #region 获得所有语义关系类型
        public async Task<List<SD_SemanticsType>> GetSemanticsType()
        {
            return await _semanticsTypeRepository.GetQuery().ToListAsync();
        }
        #endregion

        #region 获得概念树
        public async Task<TreeResult> GetWholeTree(int termClassId, string term, string cc, string sr, int deepLevel)
        {
            var termTreeModel = await GetCcTermTree(termClassId, term, cc, sr, deepLevel);
            var treeItems = termTreeModel.Select(t => new TreeItem
            {
                Id = t.TermClassId.ToString(),
                Pid = t.PId.ToString(),
                Term = t.Term,
                PathTerm = t.PathTerm,
                Source = t.Source,
                OrderIndex = t.OrderIndex

            }).ToList();
            var results = new TreeResult { TreeItems = treeItems };
            return results;
        }

        public async Task<List<TermTreeModel>> GetCcTermTree(int termClassId, string term, string cc, string sr, int deepLevel)
        {
            var termTree = await _ccTermRepository.GetQuery()
                .Where(t => t.CCCode == cc)
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
                .ToListAsync();

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

            if (deepLevel <= 0) deepLevel = 3;
            if (!string.IsNullOrEmpty(term))
            {
                var ccterm = _ccTermRepository.GetQuery()
                    .FirstOrDefault(t => t.Term == term && t.CCCode == cc);
                if (ccterm != null) termClassId = ccterm.TermClassID;
            }
            if (termClassId != -1 && termTree.Any(t => t.TermClassId == termClassId))
            {
                var newTermTree = new List<TermTreeModel>();
                var tempTerms = new List<TermTreeModel>();
                var terms = termTree.Where(t => t.TermClassId == termClassId);
                newTermTree.AddRange(terms);
                while (deepLevel-- > 1)
                {
                    if (terms.Count() <= 0) continue;
                    foreach (var item in terms)
                    {
                        var newTerms = termTree.Where(t => t.PId == item.TermClassId);
                        if (newTerms.Count() > 0) tempTerms.AddRange(newTerms);
                    }
                    if (tempTerms.Count() > 0)
                    {
                        newTermTree.AddRange(tempTerms);
                        terms = tempTerms;
                        tempTerms = new List<TermTreeModel>();
                    }
                }
                return newTermTree;
            }
            return termTree.Where(t => t.lvl <= deepLevel).ToList(); ;
        }
        #endregion

        #region 获得语义关系
        public async Task<string> Formal(string term)
        {
            if (term == null) throw new ArgumentNullException(nameof(term));

            var formalTerm = await (from tr in _termTranslationRepository.GetQuery()
                                    join cc in _ccTermRepository.GetQuery() on tr.TermClassID equals cc.TermClassID
                                    where tr.Translation.Trim() == term.Trim()
                                    select cc.Term).ToListAsync();
            return await Task.FromResult(formalTerm.Count != 0 ? formalTerm.FirstOrDefault() : term.Trim());
        }

        public async Task<List<SD_CCTerm>> GetSemantics(string term, string sr)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"term or " + "sr");

            //先获得语义正向关联的ID
            var lTermId = await _semanticsRepository.GetQuery()
                .Where(w => w.FTerm.Equals(term) && w.SR.Equals((sr)))
                .Select((s => s.LTermClassId))
                .ToListAsync();

            return lTermId.Count != 0
                  ? _ccTermRepository.GetQuery().Where(w => lTermId.Contains(w.TermClassID)).Include("SD_ConceptClass").ToList()
                  : new List<SD_CCTerm>();
        }

        public async Task<List<SD_CCTerm>> GetReverseSemantics(string term, string sr)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"term or " + "sr");

            //先获得语义反向关联的ID
            var fTermId = await _semanticsRepository.GetQuery()
                .Where(w => w.LTerm.Equals(term) && w.SR.Equals((sr)))
                .Select((s => s.FTermClassId))
                .ToListAsync();

            return fTermId.Count != 0
                  ? _ccTermRepository.GetQuery().Where(w => fTermId.Contains(w.TermClassID)).Include("SD_ConceptClass").ToList()
                  : new List<SD_CCTerm>();
        }

        #endregion

        #region 获得词库字典

        public List<WordResult> GetWholeDict()
        {
            var _ccTermRepository = GetService<IRepository<SD_CCTerm>>();
            return _ccTermRepository.GetQuery()
                .Where(t => !string.IsNullOrEmpty(t.Term))
                .Select(s => new WordResult
                {
                    Term = s.Term,
                    Cc = s.CCCode == "null" ? "UnKnown" : s.CCCode
                })
                .Distinct().ToList();
        }

        public List<WordResult> GetDictByCc(List<string> cc)
        {
            var _ccTermRepository = GetService<IRepository<SD_CCTerm>>();
            return _ccTermRepository.GetQuery()
                .Where(t => cc.Contains(t.CCCode) && !string.IsNullOrEmpty(t.Term))
                .Select(s => new WordResult
                {
                    Term = s.Term,
                    Cc = s.CCCode == "null" ? "UnKnown" : s.CCCode
                })
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// 获取翻译词词典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetTransDict()
        {
            var result = new Dictionary<string, List<string>>();
            var termTranslationRepository = GetService<IRepository<SD_TermTranslation>>();
            var _ccTermRepository = GetService<IRepository<SD_CCTerm>>();
          
            var query = termTranslationRepository.GetQuery().ToList()
                .Join(_ccTermRepository.GetQuery().ToList(), t => t.TermClassID, c => c.TermClassID, (t, c) => new
                {
                    c.Term,
                    t.Translation
                }).ToList();

            var tempResult = query
                .GroupBy(e => e.Term)
                .Select(g => new
                {
                    Term = g.Key,
                    Translations = g.Select(s => s.Translation).Distinct().ToList()
                })
                .ToDictionary(d => d.Term, d => d.Translations);
            if (tempResult != null) result = tempResult;
            return result;
        }

        /// <summary>
        /// 获取同义词字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetAliasDict()
        {
            var _semanticsRepository = GetService<IRepository<SD_Semantics>>();
            var query = _semanticsRepository.GetQuery()
                .Where(t => t.SR.Equals("D"))
                .Select(t => new { t.FTerm, t.LTerm })
                .ToList();
            var dict = new Dictionary<string, List<string>>();
            foreach (var keyValue in query)
            {
                //正向
                if (dict.ContainsKey(keyValue.FTerm))
                {
                    if (!dict[keyValue.FTerm].Contains(keyValue.LTerm) && keyValue.FTerm != keyValue.LTerm)
                        dict[keyValue.FTerm].Add(keyValue.LTerm);
                }
                else
                {
                    dict[keyValue.FTerm] = new List<string> { keyValue.LTerm };
                }


                //反向
                if (dict.ContainsKey(keyValue.LTerm))
                {
                    if (!dict[keyValue.LTerm].Contains(keyValue.FTerm) && keyValue.LTerm != keyValue.FTerm)
                        dict[keyValue.LTerm].Add(keyValue.FTerm);
                }
                else
                {
                    dict[keyValue.LTerm] = new List<string> { keyValue.FTerm };
                }
            }
            return dict;
        }

        #endregion
    }
}