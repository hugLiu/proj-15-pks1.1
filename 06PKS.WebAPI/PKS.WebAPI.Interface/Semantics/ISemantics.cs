using Jurassic.PKS.Service.Semantics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Semantics
{
    /// <summary>
    /// 语义服务接口
    /// </summary>
    public interface ISemantics
    {

        /// <summary>
        /// 简单语义分词
        /// </summary>
        /// <param name="sentence">分词短语</param>
        /// <param name="option">分词方法</param>
        /// <returns></returns>
        List<KeyWord> Segment(string sentence, SegmentOption option);

        /// <summary>
        /// 重载词库
        /// </summary>
        void ReloadDict();

        ///  <summary>
        /// 根据叙词名称获得指定概念类的叙词信息
        ///  </summary>
        ///  <param name="term">正式叙词</param>
        ///  <param name="cc">概念类</param>
        /// <returns>叙词的详细信息</returns>
        Task<List<TermInfo>> GetTermInfo(List<string> term, string cc);

        /// <summary>
        /// 根据叙词id获取指定语言的翻译词
        /// </summary>
        /// <param name="id">叙词id</param>
        /// <param name="langcode">语言类型</param>
        /// <param name="onlymain">只包含主词</param>
        /// <returns>翻译词列表</returns>
        Task<List<string>> GetTranslationById(string id, string langcode, bool onlymain);

        /// <summary>
        /// 获得树的层级结构
        /// </summary>
        /// <param name="id">叙词ID</param>
        /// <param name="cc">指定关系叙词的概念类</param>
        /// <param name="deeplevel">返回树的层级</param>
        /// <returns>树结构结果</returns>
        Task<TreeResult> Hierarchy(string id, string cc, int deeplevel);

        /// <summary>
        /// 获取 所有的概念类
        /// </summary>
        /// <returns>返回概念类表信息</returns>
        Task<List<ConceptClass>> GetCC();

        /// <summary>
        /// 获取语义关系类型（2017新增）
        /// </summary>
        /// <returns></returns>
        Task<List<SemanticsType>> GetSemanticsType();

        /// <summary>
        /// 获取 语义关系
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">指定语义关系</param>
        /// <param name="direction">语义关系方向</param>
        /// <returns>返回关联概念类的列表</returns>
        Task<List<TermInfo>> Semantics(string term, string sr, string direction);

        /// <summary>
        /// 获得指定类型词库
        /// </summary>
        /// <param name="cc">字典类型</param>
        /// <returns></returns>
        List<WordResult> GetDictionary(List<string> cc);

    }
}
