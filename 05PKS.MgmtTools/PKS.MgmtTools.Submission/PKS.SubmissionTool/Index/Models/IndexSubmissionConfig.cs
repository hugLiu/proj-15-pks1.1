using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Serialization;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using System;
using System.IO;
using System.Text;

namespace PKS.SubmissionTool.Index
{
    /// <summary>索引提交配置</summary>
    [XmlRoot("Submission")]
    public sealed class IndexSubmissionConfig
    {
        /// <summary>API服务配置</summary>
        [XmlElement]
        public XmlApiServiceConfig ApiService { get; set; }
        /// <summary>产品配置</summary>
        [XmlElement]
        public XmlProductConfig Product { get; set; }
        /// <summary>变量配置</summary>
        [XmlArray]
        [XmlArrayItem("Variable")]
        public List<XmlVariable> Variables { get; set; }
        /// <summary>标签配置</summary>
        [XmlArray]
        [XmlArrayItem("MetadataTag")]
        public List<XmlMetadataTag> MetadataTags { get; set; }
        /// <summary>载入方法</summary>
        public void Load()
        {
            if (this.ApiService == null) this.ApiService = new XmlApiServiceConfig();
            if (this.Product == null) this.Product = new XmlProductConfig();
            if (this.Variables == null) this.Variables = new List<XmlVariable>();
            if (this.MetadataTags == null) this.MetadataTags = new List<XmlMetadataTag>();
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>API服务配置</summary>
    public class XmlApiServiceConfig
    {
        /// <summary>用户名称</summary>
        [XmlElement]
        public string UserName = string.Empty;
        /// <summary>用户密码</summary>
        [XmlElement]
        public string Password = string.Empty;
        /// <summary>服务URL</summary>
        [XmlElement]
        public string Url = string.Empty;
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
        /// <summary>是否合法</summary>
        public bool IsValid()
        {
            return !this.UserName.IsNullOrEmpty() && !this.Password.IsNullOrEmpty() && !this.Url.IsNullOrEmpty();
        }
    }

    /// <summary>产品配置</summary>
    public class XmlProductConfig
    {
        /// <summary>产品文件夹</summary>
        [XmlElement]
        public string Folder = string.Empty;
        /// <summary>Excel文件</summary>
        [XmlElement]
        public string ExcelFile = string.Empty;
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>变量配置</summary>
    public class XmlVariable
    {
        /// <summary>名称</summary>
        [XmlElement]
        public string Name = string.Empty;
        /// <summary>宽度</summary>
        [XmlElement]
        public string Title = string.Empty;
        /// <summary>宽度</summary>
        [XmlElement]
        public bool BuildIn = false;
        /// <summary>值</summary>
        [XmlElement]
        public string Value = string.Empty;
        /// <summary>值提供者</summary>
        [XmlIgnore]
        public IValueProvider Provider { get; set; }
        /// <summary>获得值</summary>
        public string GetValue(object context)
        {
            if (this.Provider == null) return this.Value;
            return this.Provider.GetValue(context)?.ToString();
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>元数据标签配置</summary>
    public class XmlMetadataTag
    {
        /// <summary>名称</summary>
        [XmlElement]
        public string Name = string.Empty;
        /// <summary>元数据标签引用</summary>
        [XmlIgnore]
        public MetadataDefinition Refer { get; set; }
        /// <summary>标题</summary>
        [XmlIgnore]
        public string Title { get { return this.Refer.Title; } }
        /// <summary>启用</summary>
        [XmlElement]
        public bool Enabled = true;
        /// <summary>宽度</summary>
        [XmlElement]
        public int Width = 100;
        /// <summary>默认值</summary>
        [XmlElement]
        public string DefaultValue = string.Empty;
        /// <summary>变量集合</summary>
        [XmlIgnore]
        public Dictionary<string, XmlVariable> Variables { get; set; }
        /// <summary>枚举值集合</summary>
        [XmlElement]
        public string EnumValues = string.Empty;
        /// <summary>枚举值集合</summary>
        public string[] GetEnumValues()
        {
            return this.EnumValues.Split(',');
        }
        /// <summary>检查默认值</summary>
        public void CheckDefaultValue(List<XmlVariable> variables)
        {
            this.Variables = null;
            if (this.DefaultValue.Length == 0) return;
            var pattern = @"\{(?<name>[^\}]+)\}";
            var matches = Regex.Matches(this.DefaultValue, pattern, RegexOptions.IgnoreCase);
            if (matches.Count == 0) return;
            this.Variables = new Dictionary<string, XmlVariable>(StringComparer.Ordinal);
            foreach (Match match in matches)
            {
                var matchValue = match.Groups["name"].Value;
                var variable = variables.FirstOrDefault(e => e.Name.Equals(matchValue, StringComparison.OrdinalIgnoreCase));
                if (this.Variables.ContainsKey(match.Value)) continue;
                this.Variables[match.Value] = variable;
            }
        }
        /// <summary>生成变量值</summary>
        public string BuildVariablesValue(object context)
        {
            var sValue = this.DefaultValue;
            foreach (var pair in this.Variables)
            {
                var newValue = pair.Value.GetValue(context);
                if (newValue == null) newValue = string.Empty;
                sValue = sValue.Replace(pair.Key, newValue);
            }
            return sValue;
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>提交常数</summary>
    public static class SubmissionConsts
    {
        /// <summary>本工具名称</summary>
        public const string ToolName = "PKSSystem";
        /// <summary>唯一码</summary>
        public const string Guid = "Guid";
        /// <summary>用户名称</summary>
        public const string UserName = "PKSUserName";
        /// <summary>WebAPI站点URL</summary>
        public const string WebApiUrl = "PKSWebApiUrl";
        /// <summary>成果文件夹</summary>
        public const string ProductFolder = "ProductFolder";
        /// <summary>Excel文件名</summary>
        public const string ExcelFileName = "ExcelFileName";
        /// <summary>显示类型</summary>
        public const string ShowType = "ShowType";
        /// <summary>成果文件名</summary>
        public const string ProductFileName = "ProductFileName";
        /// <summary>规范变量名</summary>
        public static string NormalizeVariable(this string name)
        {
            return "{" + name + "}";
        }
        /// <summary>成果顺序</summary>
        public const string ST_ProductOrder = "ST_ProductOrder";
        /// <summary>文本编码</summary>
        public const string ST_TextEncoding = "ST_TextEncoding";
        /// <summary>成果文件</summary>
        public const string ST_ProductFile = "ST_ProductFile";
        /// <summary>选项</summary>
        public const string ST_Options = "ST_Options";
    }

    /// <summary>扩展名到索引数据类型值提供者</summary>
    public class ExtToIndexDataTypeValueProvider : ValueProvider
    {
        /// <summary>映射</summary>
        public static Dictionary<string, MetadataValueItem> Mapper { get; set; }
        /// <summary>生成映射</summary>
        public static void BuildMapper(MetadataDefinition tag)
        {
            var mapper = tag.Items.ToDictionary(e => e.Text.Trim(), StringComparer.OrdinalIgnoreCase);
            tag.Items.ForEach(e => mapper[e.Value.Trim()] = e);
            Mapper = mapper;
        }
        /// <summary>根据扩展名获得编码</summary>
        public static string GetEncoding(string ext)
        {
            var fileFormat = ext.GetFileFormat();
            if (fileFormat.IsStream) return string.Empty;
            return Encoding.Default.WebName;
        }
        /// <summary>根据扩展名获得选项</summary>
        public static string GetOptions(string ext)
        {
            var options = string.Empty;
            if (ExcelUtil.Support(ext))
            {
                options = ExcelTableOptions.DefaultValue;
            }
            return options;
        }
        /// <summary>获得值</summary>
        public override object GetValue(object context)
        {
            var ext = context.ToString().GetExtension();
            var showType = ext.GetFileFormat().IndexDataType;
            if (showType == null) return null;
            var showTypeItem = Mapper.GetValueOrDefaultBy(showType, null);
            if (showTypeItem == null) return null;
            return showTypeItem.Text;
        }
    }

    /// <summary>文件名值提供者</summary>
    public class FileNameValueProvider : ValueProvider
    {
        /// <summary>获得值</summary>
        public override object GetValue(object context)
        {
            return Path.GetFileNameWithoutExtension(context.ToString());
        }
    }

    /// <summary>合并上下文</summary>
    public class MergeContext
    {
        /// <summary>合并源文件</summary>
        public string MergeSourceFile;
        /// <summary>合并目标文件</summary>
        public string MergeTargetFile;
        /// <summary>合并源文件字段行行号</summary>
        public int MergeSourceFieldRow;
        /// <summary>合并源文件数据行行号</summary>
        public int MergeSourceDataRow;
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
