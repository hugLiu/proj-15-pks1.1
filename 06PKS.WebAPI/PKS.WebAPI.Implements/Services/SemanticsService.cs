using PKS.Core;
using PKS.WebAPI.Semantics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service.Semantics;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices;
using PKS.Utils;
using System.Globalization;
using PanGu;
using PanGu.Match;
using PKS.WebAPI.PanGu;
using PKS.WebAPI.Models;

namespace PKS.WebAPI.Services
{
    public class SemanticsService : AppService, ISemantics, ISingletonAppService
    {
        private readonly SemanticsProviderService _sqlProvider;
        private readonly PanGuSegmantService _panGuSegmantService;
        public SemanticsService(SemanticsProviderService sqlProvider,
                                PanGuSegmantService panGuSegmantService)
        {
            _sqlProvider = sqlProvider;
            _panGuSegmantService = new PanGuSegmantService();
        }

        public Dictionary<string, List<string>> GetBoAlias()
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            var boService = GetService<IBO2Service>();
            var bos = boService.FilterBOs(new FilterRequest());
            foreach(var bo in bos)
            {
                if (bo.Alias.Count() > 0)
                {
                    List<String> alias = new List<string>();
                    alias.Add(bo.BO);
                    alias.AddRange(bo.Alias);
                    foreach(var alia in alias)
                    {
                        var temp = alias.ToJson().JsonTo<List<string>>();
                        temp.Remove(alia);
                        dictionary[alia] = temp;
                    }
                }
            }
            return dictionary;
        }

        public void ReloadDict()
        {
            SingletonDict.Instance.ReloadDict();
        }

        public List<KeyWord> Segment(string sentence, SegmentOption option)
        {
            var dict = SingletonDict.Instance;
            sentence = sentence.ToLower();
            if (dict.NeedInitDict)
            {
                dict.TransDict = _sqlProvider.GetTransDict();
                dict.AliasDict = _sqlProvider.GetAliasDict();
                dict.AliasDict.AddRange(GetBoAlias());
                dict.NeedInitDict = false;
            }
            List<WordInfo> words;
            MatchOptions matchOptions = new MatchOptions();
            matchOptions.IgnoreCapital = true;
            if (option.MatchRule == "MinWords")
            {
                var parameter = new MatchParameter { Redundancy = 2 };
                words = _panGuSegmantService.DoSegment(sentence, matchOptions, parameter);
            }
            else
            {
                words = _panGuSegmantService.DoSegment(sentence, matchOptions);
            }

            words = words.Distinct(new WordAttributeComparer()).ToList();

            var keywords = new List<KeyWord>();
            if (option.Cc?.Count > 0)
            {
                keywords.AddRange(from item in words
                                  let cc = item.Pos.ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList()
                                  where option.Cc.Intersect(cc).Any()
                                  select new KeyWord
                                  {
                                      Term = item.Word,
                                      Cc = option.Cc.Intersect(cc).ToList()
                                  });
            }
            else
            {
                keywords.AddRange(words.Select(item => new KeyWord
                {
                    Term = item.Word,
                    Cc = item.Pos.ToString().Split('|').ToList()
                }));
            }

            foreach (var keyword in keywords)
            {
                //包含同义词
                if (option.IncludeAlias)
                {
                    foreach(var key in dict.AliasDict.Keys)
                    {
                        if(key.Equals(keyword.Term, StringComparison.OrdinalIgnoreCase))
                        {
                            keyword.Aliases.AddRange(dict.AliasDict[key]);
                        }
                    }
                    keyword.Aliases = keyword.Aliases.Distinct().ToList();
                }
                    
                        

                //包含翻译词
                if (option.IncludeTrans)
                {
                    foreach(var key in dict.TransDict.Keys)
                    {
                        if (key.Equals(keyword.Term, StringComparison.OrdinalIgnoreCase))
                        {
                            keyword.Translates.AddRange(dict.TransDict[key]);
                        }
                    }
                    keyword.Translates = keyword.Translates.Distinct().ToList();
                }
                    
            }
            return keywords;
        }

        public async Task<List<TermInfo>> GetTermInfo(List<string> terms, string cc)
        {
            if (terms == null || terms.Count == 0) throw new ArgumentNullException(nameof(terms));
            if (cc == null) throw new ArgumentNullException(nameof(cc));
            var result = new List<TermInfo>();
            foreach (var term in terms)
            {
                var dalResult = await _sqlProvider.GetTermInfo(term, cc);
                var termInfos = dalResult.Select(t => new TermInfo
                {
                    TermClassID = t.TermClassID,
                    CCCode = t.CCCode,
                    Term = t.Term,
                    PathTerm = t.PathTerm,
                    Source = t.Source,
                    Description = t.Description,
                    Remark = t.Remark
                }).ToList();
                result.AddRange(termInfos);
            }
            return result;
        }

        public async Task<List<string>> GetTranslationById(string id, string langcode, bool onlymain)
        {
            int guid = 0;
            if (!id.IsNullOrEmpty())
                guid = id.ToInt32();
            return await _sqlProvider.GetTranslationById(guid, langcode, onlymain);
        }

        public async Task<TreeResult> Hierarchy(string id, string cc, int deeplevel)
        {
            int guid = -1;
            //如果id为空，返回该概念类的整棵树
            if (!id.IsNullOrEmpty())
                guid = id.ToInt32();
            if (cc == null) throw new ArgumentNullException(nameof(cc));
            //修改，使支持扩展
            var relation = "R_" + cc + "_XF_" + cc;
            return await _sqlProvider.GetWholeTree(guid, null, cc, relation, deeplevel);
        }

        public async Task<List<ConceptClass>> GetCC()
        {
            var list = await _sqlProvider.GetCC();
            var result = list.Select(t => new ConceptClass
            {
                CCCode = t.CCCode,
                CC = t.CC,
                Tag = t.Tag,
                Type = t.Type
            }).ToList();
            return result;
        }

        public async Task<List<SemanticsType>> GetSemanticsType()
        {
            var list = await _sqlProvider.GetSemanticsType();
            var result = list.Select(t => new SemanticsType
            {
                SR = t.SR,
                CCCode1 = t.CCCode1,
                CCCode2 = t.CCCode2,
                Description = t.Description,
                Remark = t.Remark
            }).ToList();
            return result;
        }


        public async Task<List<TermInfo>> Semantics(string term, string sr, string direction)
        {
            term = await _sqlProvider.Formal(term);
            sr = sr.ToString(CultureInfo.InvariantCulture);
            List<SD_CCTerm> list;
            switch (direction)
            {
                case "forward": list = await _sqlProvider.GetSemantics(term, sr); break;
                case "backward": list = await _sqlProvider.GetReverseSemantics(term, sr); break;
                default: throw new ArgumentException(@"direction");
            }
            var result = list.Select(t => new TermInfo
            {
                TermClassID = t.TermClassID,
                CCCode = t.CCCode,
                Term = t.Term,
                PathTerm = t.PathTerm,
                Source = t.Source,
                Description = t.Description,
                Remark = t.Remark
            }).ToList();
            return result;
        }

        public List<WordResult> GetDictionary(List<string> cc)
        {
            if (cc == null || cc.Count == 0)
            {
                return  _sqlProvider.GetWholeDict();
            }
            return  _sqlProvider.GetDictByCc(cc);
        }

        
    }
}
