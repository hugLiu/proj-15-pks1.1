using Jurassic.AppCenter.Caches;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using Jurassic.WebQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 配合数据采集组件的配置管理类，管理列表和编辑界面的各项参数
    /// </summary>
    public class AdvDataConfigManager
    {
        static CachedList<AdvDataConfig> _configCache = new CachedList<AdvDataConfig>();
        static bool _modified = false;
        static object _synObj = new object();

        /// <summary>
        /// 根据指定的类型名称和属性名称获取对应的属性配置项
        /// </summary>
        /// <param name="className">类型名称</param>
        /// <param name="propName">属性名称</param>
        /// <param name="attr">标签属性</param>
        /// <returns>属性配置项</returns>
        public static AdvDataConfigItem GetPropertyConfig(CatalogExtAttribute attr, string className, string propName)
        {
            AdvDataConfigItem foundItem = null;
            var classConfig = _configCache.FirstOrDefault(cfg => cfg.ClassName.Equals(className));
            if (classConfig == null)
            {
                classConfig = new AdvDataConfig() { ClassName = className };
                _configCache.Add(classConfig);
                _modified = true;
            }
            foundItem = classConfig.Items.FirstOrDefault(c => c.PropertyName == propName);
            if (foundItem == null)
            {
                foundItem = new AdvDataConfigItem
                {
                    OverWrite = true,
                    FormOrder = 1,
                    PropertyName = propName,
                    GridOrder = 1,

                    AllowNull = attr.AllowNull,
                    DataSource = attr.DataSource.ToStr(),
                    DataSourceType = CommOp.ToStr(attr.DataSourceType),
                    DataType = attr.DataType.ToString(),
                    MaxLength = attr.MaxLength,
                    MinLength = attr.MinLength,
                    MaxValue = CommOp.ToStr(attr.MaxValue),
                    MinValue = CommOp.ToStr(attr.MinValue),
                    RegExpr = attr.RegExpr,
                    DefaultValue = attr.DefaultValue,
                    InputFormat = attr.InputFormat,
                    DisplayFormat = attr.DisplayFormat,
                    Browsable = attr.Browsable,
                };
                classConfig.Items.Add(foundItem);
                _modified = true;
            }
            else if (foundItem.OverWrite)
            {
                attr.DefaultValue = foundItem.DefaultValue;
                attr.DataType = CommOp.ToEnum<ExtDataType>(foundItem.DataType);
                attr.AllowNull = foundItem.AllowNull;
                attr.DataSource = foundItem.DataSource;
                attr.DataSourceType = CommOp.ToEnum<ExtDataSourceType>(foundItem.DataSourceType);
                attr.MaxLength = foundItem.MaxLength;
                attr.MinLength = foundItem.MinLength;
                attr.MaxValue = foundItem.MaxValue;
                attr.MinValue = foundItem.MinValue;
                attr.RegExpr = foundItem.RegExpr;
                attr.InputFormat = foundItem.InputFormat;
                attr.DisplayFormat = foundItem.DisplayFormat;
                attr.Browsable = foundItem.Browsable;
            }

            return foundItem;
        }

        /// <summary>
        /// 当配置数据发生修改时保存配置到配置文件
        /// </summary>
        public static void Save()
        {
            lock (_synObj)
            {
                if (_modified)
                {
                    _configCache.Save();
                    _modified = false;
                }
            }
        }
    }
}