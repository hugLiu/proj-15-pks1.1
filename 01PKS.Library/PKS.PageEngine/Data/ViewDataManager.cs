using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PKS.PageEngine.EvenHandlers;
using PKS.PageEngine.Extensions;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;
using PKS.PageEngine.View;

namespace PKS.PageEngine.Data
{
    /// <summary>
    /// 组织组件所需数据
    /// </summary>
    public class ViewDataManager
    {
        private PageContext _pageContext;

        public ViewDataManager(PageContext pageContext)
        {
            this._pageContext = pageContext;
        }

        public JsDataModel GenerateComponentDataModel(Dictionary<string, ViewComponent> coms)
        {
            JsDataModel model = new JsDataModel();
            foreach (var comKVP in coms)
            {
                var component = comKVP.Value;
                var comId = component.Id;
                component.BeforeLoadData();

                Dictionary<string, object> comParamsDic =
                    new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                var sysParameters = component.FragmentInfo.ComponentParameters;
                //替换查询输入参数变量
                ReplaceQueryVariableParam(component.FragmentInfo.QueryPlan);
                component.LoadData();

               

                foreach (var componentParam in sysParameters)
                {
                    //如果为Es输出参数，忽略
                    if (!string.IsNullOrWhiteSpace(componentParam.Metadata))
                        continue;
                    //if (componentParam.Code == "text")
                    //{
                    //    var value = component.FragmentInfo.Htmltext??Convert.ToString(componentParam.DefaultValue);
                    //    //if (component.FragmentInfo.FragmentHasTextTemplate)
                    //    //{
                    //    //    value = ReplaceTextTemplateVariable(component.FragmentInfo, value);
                    //    //}
                    //    comParamsDic.Add(componentParam.Code, value);
                    //    continue;
                    //}
                    //组件默认data和items为对应的数据源属性
                    if (componentParam.Code == "data" || componentParam.Code == "items" || componentParam.Code == "datasource")
                    {
                        var value = component.Data;
                        if (component.FragmentInfo.FragmentHasTextTemplate)
                        {
                            value = ReplaceTextTemplateVariable(component.FragmentInfo, component.FragmentInfo.Htmltext);
                        }
                        comParamsDic.Add(componentParam.Code, value);
                        continue;
                    }
                    if (componentParam.Code == "item")
                    {
                        var value = component.Data;
                        if (component.FragmentInfo.FragmentHasTextTemplate)
                        {
                            value = ReplaceTextTemplateVariable(component.FragmentInfo, value).FirstOrDefault();
                        }
                        else
                        {
                            if (value != null)
                            {
                                if (value is JArray)
                                {
                                    value = (value as JArray).FirstOrDefault();
                                }
                                else
                                {
                                    if (value is IList)
                                    {
                                        var list = (value as IList);
                                        if (list.Count > 0)
                                            value = list[0];
                                    }
                                }
                            }
                        }
                        comParamsDic.Add(componentParam.Code, value);
                        continue;
                    }
                    if (componentParam.Code == "map")
                    {
                        //键值对
                        var mapValue = BuildComponentMapPropertyValue(component.FragmentInfo);
                        comParamsDic.Add(componentParam.Code, mapValue);
                        continue;
                    }
                    if (componentParam.Code == "title")
                    {
                        var title = string.IsNullOrWhiteSpace(component.FragmentInfo.Title)
                            ? Convert.ToString(componentParam.Value ?? componentParam.DefaultValue)
                            : component.FragmentInfo.Title;
                        comParamsDic.Add(componentParam.Code, title);
                        continue;
                    }
                    if(componentParam.Code=="aname")
                    {
                        //todo 临时处理
                        var tagName = component.FullTagName;
                        if (tagName=="pks:headline")
                        {
                            var placeHolderId = component.FragmentInfo.PlaceHolderId;
                            var catalogItem = _pageContext.CatalogueItems.FirstOrDefault(item => item.PlaceHolderId == placeHolderId);
                            if(catalogItem!=null)
                                comParamsDic.Add(componentParam.Code, catalogItem.Code.Replace(".","-"));
                            continue;
                        }
                    }
                    if (componentParam.Code == "text")
                    {
                        if (component.FragmentInfo.FragmentHasTextTemplate)
                        {
                            var value = component.FragmentInfo.Htmltext ?? Convert.ToString(componentParam.DefaultValue);
                            var htmlValue = ReplaceTextTemplateVariable(component.FragmentInfo, value);
                            comParamsDic.Add(componentParam.Code, htmlValue);
                            continue;
                        }
                        var title = string.IsNullOrWhiteSpace(component.FragmentInfo.Title)
                            ? Convert.ToString(componentParam.Value ?? componentParam.DefaultValue)
                            : component.FragmentInfo.Title;
                        comParamsDic.Add(componentParam.Code, title);
                        continue;
                    }                 

                    var comValue = componentParam.Value ?? componentParam.DefaultValue;
                    if (comValue != null&& componentParam.DataType!=null&& componentParam.DataType.ToLower()=="string")
                    {
                        comValue = ReplaceComponentVariableParam(comValue.ToString());
                    }
                    comParamsDic.Add(componentParam.Code, comValue.ToSimpleType(componentParam.DataType));
                }
                component.AfterLoadData();
                model.Add(Consts.CModelPropertyPrefix + comId, comParamsDic);
            }
            return model;
        }

        /// <summary>
        /// 替换查询参数中的上下文参数
        /// </summary>
        /// <param name="queryPlan"></param>
        public void ReplaceQueryVariableParam(QueryPlan queryPlan)
        {
            if (queryPlan == null)
                return;
            if (queryPlan.Fields != null)
            {
                foreach (var queryPlanField in queryPlan.Fields)
                {
                    if (queryPlanField.FieldQueryType == FieldQueryType.Fixed)
                        queryPlanField.FieldValue = queryPlanField.Value;
                    else
                    {
                        var contextParamValue= _pageContext.GetContextParamValue(queryPlanField.Value.Replace("@", ""));
                        if (contextParamValue == null)
                        {
                            queryPlanField.FieldValue = queryPlanField.Value;
                        }
                        else
                        {
                            queryPlanField.FieldValue = contextParamValue;
                        }
                    }
                       
                            
                }
            }
        }
        /// <summary>
        /// 替换组件参数中的上下文参数
        /// </summary>
        /// <param name="queryPlan"></param>
        public string ReplaceComponentVariableParam(string componentParam)
        {
            if (componentParam.StartsWith("@"))
            {
                var paramName = componentParam.Replace("@", "");
                var paramValue= _pageContext.GetContextParamValue(paramName);
                if (paramValue == null)
                    return componentParam;
                return Convert.ToString(paramValue);
            }
            return componentParam;
        }

        /// <summary>
        /// 构建组件Map属性值
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> BuildComponentMapPropertyValue(FragmentInfo fragmentInfo)
        {
            if (fragmentInfo.QueryOutputParams == null || fragmentInfo.QueryOutputParams.Count == 0)
                return null;
            Dictionary<string, string> mapValue = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var fragmentInfoQueryOutputParam in fragmentInfo.QueryOutputParams)
            {
                if (!mapValue.ContainsKey(fragmentInfoQueryOutputParam.Code))
                    mapValue.Add(fragmentInfoQueryOutputParam.Code, fragmentInfoQueryOutputParam.Metadata);
            }
            return mapValue;
        }

        /// <summary>
        /// 替换模板变量
        /// </summary>
        /// <returns></returns>
        private List<string> ReplaceTextTemplateVariable(FragmentInfo fragmentInfo, object textTemplateData)
        {
            //todo 代码整理
            List<string> textList = new List<string>();
            var htmlText = ReplaceWithPageContextParam(fragmentInfo.Htmltext);
            //参数替换
            if (fragmentInfo.FragmentHasTextTemplate && !string.IsNullOrWhiteSpace(htmlText) &&
                _pageContext.TextTemplateParams != null && textTemplateData != null)
            {
                if (textTemplateData is JArray)
                {
                    return ReplaceTextTemplateVariableWithJArray(textTemplateData as JArray, htmlText);
                }
                else
                {
                    return ReplaceTextTemplateVariableWithObj(textTemplateData, htmlText);
                }
            }
            return textList;
        }

        private List<string> ReplaceTextTemplateVariableWithJArray(JArray obj, string htmlText)
        {
            List<string> textList = new List<string>();
            foreach (var arrayItem in obj)
            {
                var newHtmlText = string.Copy(htmlText);
                var children = arrayItem.Children();
                var jProperties = children.Select(item => item as JProperty);
                foreach (var templateParam in _pageContext.TextTemplateParams)
                {
                    var propertyInfo =
                        jProperties.FirstOrDefault(
                            item => string.Equals(item.Name, templateParam.Code, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    var v = Convert.ToString(propertyInfo.Value);
                    newHtmlText = newHtmlText.Replace("%" + templateParam.Name + "%", v);
                    newHtmlText = newHtmlText.Replace("%" + templateParam.Code + "%", v);
                }
                textList.Add(newHtmlText);
            }
            return textList;
        }

        private string ReplaceWithPageContextParam(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
                return htmlText;
            if (_pageContext.ContextParams != null)
            {
                foreach (var pageContextContextParam in _pageContext.ContextParams)
                {
                    var value = Convert.ToString(pageContextContextParam.Value.Value);
                    htmlText = htmlText.Replace("%" + pageContextContextParam.Key + "%", value);
                }
                //如果文本模板变量的值在上下文参数中，直接替换
                foreach (var templateParam in _pageContext.TextTemplateParams)
                {
                    var code = templateParam.Code;
                    var contextValue= _pageContext.GetContextParamValue<string>(code);
                    if(!string.IsNullOrWhiteSpace(contextValue))
                    {
                        htmlText = htmlText.Replace("%" + templateParam.Name + "%", contextValue);
                        htmlText = htmlText.Replace("%" + templateParam.Code + "%", contextValue);
                    }
                }
            }
            return htmlText;
        }

        private List<string> ReplaceTextTemplateVariableWithObj(object obj, string htmlText)
        {
            Func<object, string, string> replaceObjFunc = ((item, text) =>
            {
                var newHtmlText = text;
                foreach (var templateParam in _pageContext.TextTemplateParams)
                {
                    var propertyInfo = item.GetType()
                        .GetProperty(templateParam.Code, BindingFlags.IgnoreCase);
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    var v = Convert.ToString(propertyInfo.GetValue(obj, null));
                    newHtmlText = newHtmlText.Replace("%" + templateParam.Name + "%", v);
                    newHtmlText = newHtmlText.Replace("%" + templateParam.Code + "%", v);

                }
                return newHtmlText;
            });

            List<string> textList = new List<string>();
            System.Collections.ICollection Ilist = obj as System.Collections.ICollection;
            if (Ilist != null)
            {

                foreach (object itemObj in Ilist)
                {
                    textList.Add(replaceObjFunc(itemObj, htmlText));
                }
            }
            else
            {
                textList.Add(replaceObjFunc(obj, htmlText));
            }
            return textList;
        }
    }
}
