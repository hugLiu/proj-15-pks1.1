using Jurassic.PKS.Service.Semantics;
using Jurassic.PKS.WebAPI.Semantics;
using PanGu;
using PKS.WebAPI.Models;
using PKS.WebAPI.Semantics;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PKS.WebAPI.Controllers
{
    /// <summary>语义服务控制器</summary>
    public class SemanticServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public SemanticServiceController(ISemantics service, IBO2Service bo2Service)
        {
            ServiceImpl = service;
            BO2ServiceImpl = bo2Service;
        }

        /// <summary>服务实例</summary>
        private ISemantics ServiceImpl { get; }
        private IBO2Service BO2ServiceImpl { get; }

        /// <summary>获得服务信息</summary>
        [HttpGet]
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "语义服务主要用于叙词分词、获得同义词，翻译词，概念类的树层次结构等"
            };
        }

        /// <summary>重载词库</summary>
        [HttpGet]
        public void ReloadDict()
        {
            ServiceImpl.ReloadDict();
        }

        /// <summary>简单语义分词</summary>
        [HttpPost]
        public List<KeyWord> Segment(SegmentRequest request)
        {
            return this.ServiceImpl.Segment(request.Sentence, request.Option);
        }
        /// <summary>根据叙词名称获得指定概念类的叙词信息</summary>
        [HttpPost]
        public async Task<List<TermInfo>> GetTermInfo(GetTermInfoRequest request)
        {
            return await this.ServiceImpl.GetTermInfo(request.Term, request.Cc);
        }

        /// <summary>根据叙词id返回对应的翻译词</summary>
        [HttpGet]
        public async Task<List<string>> GetTranslationById([FromUri]GetTranslationByIDRequest request)
        {
            return await this.ServiceImpl.GetTranslationById(request.Id, request.LangCode, request.OnlyMain);
        }

        /// <summary>根据叙词id返回对应的子树的层级结构，并限定返回叙词的概念类</summary>
        [HttpGet]
        public async Task<TreeResult> Hierarchy([FromUri]HierarchyRequest request)
        {
            if (request == null)
            {
                return null;
            }
            return await this.ServiceImpl.Hierarchy(request.Id, request.Cc, request.DeepLevel);
        }

        /// <summary>获取所有的概念类</summary>
        [HttpGet]
        public async Task<List<ConceptClass>> GetCC()
        {
            return await this.ServiceImpl.GetCC();
        }

        /// <summary>获取所有语义关系类型</summary>
        [HttpGet]
        public async Task<List<SemanticsType>> GetSemanticsType()
        {
            return await this.ServiceImpl.GetSemanticsType();
        }

        /// <summary>获取语义关系</summary>
        [HttpGet]
        public async Task<List<TermInfo>> Semantics([FromUri]SemanticsRequest request)
        {
            return await this.ServiceImpl.Semantics(request.Term, request.SR, request.Direction);
        }

        /// <summary>获得指定类型词库</summary>
        [HttpPost]
        public List<string> GetDictionary(GetDictionaryRequest request)
        {
            List<WordResult> result;
            if (request == null) result =  this.ServiceImpl.GetDictionary(null);
            else result =  this.ServiceImpl.GetDictionary(request.Cc);
            return result.Select(t => t.Term).ToList();
        }

        /// <summary>获取盘古分词词库</summary>
        [HttpGet]
        public List<WordAttribute> GetDic4PanGu()
        {
            var result =  this.ServiceImpl.GetDictionary(null);
            var waList = new List<WordAttribute>();
            foreach (var item in result)
            {
                waList.Add(new WordAttribute { Word = item.Term, Pos = (POS)Enum.Parse(typeof(POS), item.Cc) });
            }
            // 获取bo数据
            var query = BO2ServiceImpl.FilterBOs(new FilterRequest());
            foreach(var bo in query)
            {
                waList.Add(new WordAttribute { Word = bo.BO, Pos = (POS)Enum.Parse(typeof(POS), "BO") });
                foreach(var alia in bo.Alias)
                {
                    waList.Add(new WordAttribute { Word = alia, Pos = (POS)Enum.Parse(typeof(POS), "BO") });
                }                
            }
            return waList;
        }
    }
}
