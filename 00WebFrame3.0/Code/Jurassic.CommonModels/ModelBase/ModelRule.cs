using Jurassic.AppCenter.Resources;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Jurassic.CommonModels.EntityBase;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 用于全面描述一个数据模型的规则和约束的类
    /// </summary>
    public class ModelRule
    {
        static Dictionary<Type, ModelRule> _ruleCache = new Dictionary<Type, ModelRule>();
        static object _synObj = new object();

        /// <summary>
        /// 清除缓存，重新读取元数据
        /// </summary>
        public static void Clear()
        {
            _ruleCache.Clear();
        }

        /// <summary>
        /// 根据某类型，返回一个规则类
        /// </summary>
        /// <param name="type">类型参数</param>
        /// <returns>规则类</returns>
        public static ModelRule Get(Type type)
        {
            if (!_ruleCache.ContainsKey(type))
            {
                lock (_synObj)
                {
                    var rule = new ModelRule(type);
                    _ruleCache[type] = rule;
                }
            }
            return _ruleCache[type];
        }

        /// <summary>
        /// 根据某类型，返回一个规则描述类
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <returns>规则类</returns>
        public static ModelRule Get<T>()
        {
            return Get(typeof(T));
        }

        private ModelRule(Type modelType)
        {
            ModelType = modelType;
            CollectionRules = new List<ModelRule>();
            GetModelRules(modelType);
        }

        /// <summary>
        /// 解析对象以后生成的标签对象，可能是来自于对象自身的标签，
        /// 在没有打标签时是根据对象本身的定义信息生成的
        /// </summary>
        public CatalogExtAttribute Attr { get; set; }

        /// <summary>
        /// 数据模型的类型
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// 规则名称，等同于属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象中所有单个属性的规则集
        /// </summary>
        public ICollection<CatalogExtAttribute> SingleRules { get; set; }

        /// <summary>
        /// 对象中所有集合属性的规则集，集合必须是泛型类的集合
        /// </summary>
        public ICollection<ModelRule> CollectionRules { get; set; }

        /// <summary>
        /// 批量获取某个对象下所有非集合属性的数据规则和约束
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        void GetModelRules(Type modelType)
        {
            Attr = modelType.GetCustomAttributes(true).FirstOrDefault(attr => attr is CatalogExtAttribute) as CatalogExtAttribute ?? new CatalogExtAttribute();
            SingleRules = GetSingleModelRules(modelType).ToList();
            foreach (var prop in modelType.GetProperties())
            {
                var attrs = prop.GetCustomAttributes(true);

                //只处理非字符串的集合类型
                if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                {
                    Type itemType = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                    //如果属性是非泛型集合类型，则pass掉
                    if (itemType == null || itemType == typeof(Sys_DataLanguage))
                    {
                        continue;
                    }
                    CatalogExtAttribute extAttr = attrs.FirstOrDefault(attr => attr is CatalogExtAttribute) as CatalogExtAttribute;

                    if (extAttr != null && (extAttr.Browsable == false || extAttr.Editable == false || extAttr.DataSourceType == ExtDataSourceType.Hidden))
                    {
                        continue;
                    }
                    ModelRule rule = new ModelRule(itemType);
                    rule.Name = prop.Name;
                    rule.Attr.Property = prop;

                    CollectionRules.Add(rule);
                }
            }
        }

        /// <summary>
        /// 获取某对象的单个属性值
        /// </summary>
        /// <param name="obj">符合该类所定义的类型的对象</param>
        /// <param name="propName">单个属性的名称</param>
        /// <returns>单个属性的值</returns>
        public object GetSingleValue(object obj, string propName)
        {
            foreach (var attr in SingleRules)
            {
                if (attr.Name == propName)
                {
                    return attr.Property.GetValue(obj, null);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取某对象的集合属性值
        /// </summary>
        /// <param name="obj">符合该类所定义的类型的对象</param>
        /// <param name="propName">集合属性名称</param>
        /// <returns>集合属性值</returns>
        public object GetCollectionValue(object obj, string propName)
        {
            foreach (var prop in CollectionRules)
            {
                if (prop.Name == propName)
                {
                    return prop.Attr.Property.GetValue(obj, null);
                }
            }
            return null;
        }

        IEnumerable<CatalogExtAttribute> GetSingleModelRules(Type modelType)
        {
            foreach (var prop in modelType.GetProperties())
            {
                //只处理值类型、可为空类型和字符串型
                if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(String)
                     || prop.PropertyType.IsNullableType())
                {
                    yield return GetAdvDataRule(prop);
                }
            }
        }

        /// <summary>
        /// 返回属性的数据规则和约束条件
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        CatalogExtAttribute GetAdvDataRule(PropertyInfo prop)
        {
            var attrs = prop.GetCustomAttributes(true);

            CatalogExtAttribute extAttr = attrs.FirstOrDefault(attr => attr is CatalogExtAttribute) as CatalogExtAttribute;

            if (extAttr == null)
            {
                extAttr = new CatalogExtAttribute();
                if (prop.Name == "Id")
                {
                    extAttr.Editable = false;
                    extAttr.DataType = ExtDataType.SingleLineText;
                }
                if (prop.PropertyType.IsValueType && !prop.PropertyType.IsNullableType())
                {
                    extAttr.AllowNull = false;
                }
            }

            if (extAttr.Name.IsEmpty())
            {
                extAttr.Name = prop.Name;
            }

            RequiredAttribute required = attrs.FirstOrDefault(attr => attr is RequiredAttribute) as RequiredAttribute;

            if (required != null)
            {
                extAttr.AllowNull = false;
            }

            StringLengthAttribute stringLen = attrs.FirstOrDefault(attr => attr is StringLengthAttribute) as StringLengthAttribute;

            if (stringLen != null)
            {
                extAttr.MaxLength = stringLen.MaximumLength;
                extAttr.MinLength = stringLen.MinimumLength;
            }

            DisplayColumnAttribute displayProp = attrs.FirstOrDefault(attr => attr is DisplayColumnAttribute) as DisplayColumnAttribute;

            if (displayProp != null)
            {
                extAttr.DisplayProperty = displayProp.DisplayColumn;
            }

            DisplayFormatAttribute displayFmt = attrs.FirstOrDefault(attr => attr is DisplayFormatAttribute) as DisplayFormatAttribute;

            if (displayFmt != null)
            {
                extAttr.DisplayFormat = displayFmt.DataFormatString;
            }

            RegularExpressionAttribute reg = attrs.FirstOrDefault(attr => attr is RegularExpressionAttribute) as RegularExpressionAttribute;

            if (reg != null)
            {
                extAttr.RegExpr = reg.Pattern;
            }

            RangeAttribute rng = attrs.FirstOrDefault(attr => attr is RangeAttribute) as RangeAttribute;

            if (rng != null)
            {
                extAttr.MinValue = rng.Minimum;
                extAttr.MaxValue = rng.Maximum;
            }

            DisplayAttribute dsp = attrs.FirstOrDefault(attr => attr is DisplayAttribute) as DisplayAttribute;

            if (dsp != null)
            {
                extAttr.Name = dsp.Name;
            }

            BrowsableAttribute brw = attrs.FirstOrDefault(attr => attr is BrowsableAttribute) as BrowsableAttribute;

            if (brw != null)
            {
                extAttr.Browsable = brw.Browsable;
            }
            if (extAttr.DataType == ExtDataType.Auto)
            {
                Type type = prop.PropertyType;
                if (type.IsNullableType())
                {
                    type = type.GetGenericArguments()[0];
                }
                if (type.IsEnum)
                {
                }
                else if (type == typeof(DateTime))
                {
                    extAttr.DataType = ExtDataType.Date;
                }
                else if (type == typeof(bool))
                {
                    extAttr.DataType = ExtDataType.Bool;
                }
                else if (type == typeof(decimal))
                {
                    extAttr.DataType = ExtDataType.Currency;
                }
                else if (type == typeof(float) || type == typeof(double))
                {
                    extAttr.DataType = ExtDataType.FloatNumber;
                }
                else if (type.IsValueType)
                {
                    extAttr.DataType = ExtDataType.SingleNumber;
                }
            }
            if (!extAttr.LinkedProperty.IsEmpty() && extAttr.DisplayProperty.IsEmpty())
            {
                extAttr.DisplayProperty = extAttr.Name;
            }

            if (extAttr.DisplayFormat == null)
            {
                switch (extAttr.DataType)
                {
                    case ExtDataType.Date:
                        extAttr.DisplayFormat = "yyyy-MM-dd";
                        break;
                    case ExtDataType.DateAndTime:
                        extAttr.DisplayFormat = "yyyy-MM-dd HH:mm:ss";
                        break;
                    case ExtDataType.FloatNumber:
                        extAttr.DisplayFormat = "#0.00";
                        //extAttr.MaxValue = int.MaxValue;
                        break;
                    case ExtDataType.SingleNumber:
                        extAttr.DisplayFormat = "#0";
                        //extAttr.MaxValue = int.MaxValue;
                        break;
                    case ExtDataType.Currency:
                        char currChar = 100.ToString("C")[0];
                        extAttr.DisplayFormat = currChar + "#0.00";
                        //extAttr.MaxValue = int.MaxValue;
                        break;
                    case ExtDataType.Percent:
                        extAttr.DisplayFormat = "p2";
                        //extAttr.MaxValue = 1;
                        break;
                    case ExtDataType.Time:
                        extAttr.DisplayFormat = "H:mm";
                        break;
                }
            }
            if (extAttr.InputFormat == null)
            {
                extAttr.InputFormat = extAttr.DisplayFormat;
            }
            switch (extAttr.DataType)
            {
                case ExtDataType.FloatNumber:
                case ExtDataType.SingleNumber:
                case ExtDataType.Currency:
                case ExtDataType.Percent:
                    if (extAttr.MaxValue == null && extAttr.MinValue == null)
                    {
                        extAttr.MinValue = 0;
                        extAttr.MaxValue = int.MaxValue;
                    }
                    break;
            }
            extAttr.Property = prop;
            return extAttr;
        }

        /// <summary>
        /// 根据子项类型获取某对象的集合属性值
        /// </summary>
        /// <typeparam name="TItem">集合中子项类型</typeparam>
        /// <param name="obj">主对象</param>
        /// <returns>集合属性的值</returns>
        public object GetCollectionValue<TItem>(object obj)
        {
            var collRule = CollectionRules.FirstOrDefault(rule => rule.ModelType == typeof(TItem));
            if (collRule == null)
            {
                return null;
            }
            return GetCollectionValue(obj, collRule.Name);
        }
    }
}
