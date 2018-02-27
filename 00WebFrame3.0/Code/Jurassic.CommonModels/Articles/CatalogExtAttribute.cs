using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Jurassic.AppCenter;
using Jurassic.Com;
using System.Data;
using System.Reflection;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 在栏目扩展属性初始化时，该扩展属性的详细定义
    /// ps: 定义某个属性的约束、录入和显示方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class CatalogExtAttribute : Attribute
    {
        public CatalogExtAttribute()
        {
            AllowNull = true;
            Browsable = true;
            Editable = true;
            DataType = ExtDataType.Auto;
        }

        public PropertyInfo Property { get; set; }

        public string Name { get; internal set; }

        public ExtDataType DataType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 提供给用户选项的数据源类型
        /// </summary>
        public ExtDataSourceType DataSourceType { get; set; }

        private string _dataSource;
        /// <summary>
        /// 用户选项的数据源描述
        /// </summary>
        public string DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                if (DataSourceType == ExtDataSourceType.None && !String.IsNullOrWhiteSpace(_dataSource))
                {
                    DataSourceType = ExtDataSourceType.DirectList;
                }
            }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 是否允许空值
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue { get; set; }

        /// <summary>
        /// 正则表达式, 用于验证输入
        /// </summary>
        public string RegExpr { get; set; }

        /// <summary>
        /// 排序位
        /// </summary>
        public int Ord { get; set; }
        
        /// <summary>
        /// 是否强行改写已存在的属性
        /// </summary>
        public bool ForceUpdate { get; set; }

        //public DataTable GetUserDefineData()
        //{
        //    RefHelper.LoadClass(DataSource);
        //}

        /// <summary>
        /// 是否可在列表中出现,默认为True
        /// 也决定是否可搜索
        /// </summary>
        public bool Browsable { get; set; }

        /// <summary>
        /// 是否可编辑，默认为True
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// 对应数据实体的类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 关联的属性名称，用于下拉列表联动
        /// </summary>
        public string LinkedProperty { get; set; }

        /// <summary>
        /// 用于联动的下拉列表时，初始显示哪个属性的值
        /// </summary>
        public string DisplayProperty { get; set; }

        /// <summary>
        /// 录入时的格式化信息
        /// </summary>
        public string InputFormat { get; set; }

        /// <summary>
        /// 显示时的格式化信息
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// 指定利用该属性在查询中进行用户级权限控制，用于表示用户ID的属性
        /// </summary>
        public bool AuthByUser { get; set; }

        /// <summary>
        /// 指定利用该属性在查询中进行部门级权限控制，用于表示部门ID的属性
        /// </summary>
        public bool AuthByDept { get; set; }
    }
}
