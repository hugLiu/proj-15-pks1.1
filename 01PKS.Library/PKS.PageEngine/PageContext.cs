using System;
using System.Collections.Generic;
using System.Linq;
using PKS.PageEngine.Param;

namespace PKS.PageEngine
{
    public class PageContext
    {
        private Dictionary<string,VParam> _contextParams;
        private List<ComponentParam> _comParams;
        private List<TextTemplateParam> _textTemplateParams;
        /// <summary>
        /// 上下文参数
        /// </summary>
        public Dictionary<string, VParam> ContextParams
        {
            get { return _contextParams; }
        }

        /// <summary>
        /// 所有组件参数
        /// </summary>
        public List<ComponentParam>  ComponentParams
        {
            get { return _comParams; }
        }
        /// <summary>
        /// 所有文本模板参数
        /// </summary>
        public List<TextTemplateParam> TextTemplateParams
        {
            get { return _textTemplateParams; }
        }
        public PageContext()
        {
            _contextParams=new Dictionary<string, VParam>(StringComparer.OrdinalIgnoreCase);
            _comParams=new List<ComponentParam>();
            _textTemplateParams=new List<TextTemplateParam>();
        }

        /// <summary>
        /// 新增组件参数
        /// </summary>
        /// <param name="comParams"></param>
        public void AddComponentParams(IEnumerable<ComponentParam> comParams)
        {
            _comParams.AddRange(comParams);
        }

        /// <summary>
        /// 新增文本模板参数
        /// </summary>
        /// <param name="textTemlateParams"></param>
        public void AddTextParams(IEnumerable<TextTemplateParam> textTemlateParams)
        {
            _textTemplateParams.AddRange(textTemlateParams);
        }

        public void RebuildParams()
        {
            //var fragTypeParams = JsonConvert.DeserializeObject<List<FragmentTypeParam>>(strSysParam);
            //var componentSysParams = new List<ComponentParam>();
            //foreach (var fragmentTypeParam in fragmentTypeParams)
            //{
            //    ComponentParam sysParam = new ComponentParam();
            //    sysParam.Name = fragmentTypeParam.Name;
            //    sysParam.Code = fragmentTypeParam.Code;
            //    sysParam.DataType = fragmentTypeParam.DataType;
            //    sysParam.DefaultValue = fragmentTypeParam.DefaultValue;
            //    var fragTypeParam = fragTypeParams.FirstOrDefault(
            //        item => string.Equals(item.Code, fragmentTypeParam.Code, StringComparison.OrdinalIgnoreCase));
            //    if (fragTypeParam != null)
            //    {
            //        sysParam.Value = fragTypeParam.Value;
            //    }
            //}
            //return componentSysParams;


        
        }
        //todo 
       public List<CatalogueItem> CatalogueItems { get; set; }
    }
}
