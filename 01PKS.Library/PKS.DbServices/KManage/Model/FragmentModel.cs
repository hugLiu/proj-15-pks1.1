using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PKS.DbModels;
using PKS.Utils;

namespace PKS.DbServices.KManage.Model
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class FragmentModel
    {
        public int Id { get; set; }

        public int? TemplateCatalogueId { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 查询参数
        /// </summary>
        public string QueryParameter { get; set; }
        /// <summary>
        /// 组件参数
        /// </summary>
        public string ComponentParameter { get; set; }
        /// <summary>
        /// Html文本
        /// </summary>
        public string Htmltext { get; set; }

        public int FragmentTypeId { get; set; }

        /// <summary>
        /// 组件占位唯一标识
        /// </summary>
        public string PlaceholderId { get; set; }

        /// <summary>
        /// 知识片段类型
        /// </summary>
        public string FragmentTypeCode { get; set; }

        /// <summary>
        /// 知识片段名称
        /// </summary>
        public string FragmentTypeName { get; set; }

        /// <summary>
        /// 知识片段对应Vue标签
        /// </summary>
        public string FragmentVueTag { get; set; }

        /// <summary>
        /// 知识片段是否为文本模板
        /// </summary>
        public bool FragmentHasTextTemplate { get; set; }

        /// <summary>
        /// 知识片段所在模板
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// 从属标题组件的placeholder【对应文档目录的placeholder】
        /// 注：在标题之下的知识片段
        /// </summary>
        public string CatalogueNodeId { get; set; }
    }
}
