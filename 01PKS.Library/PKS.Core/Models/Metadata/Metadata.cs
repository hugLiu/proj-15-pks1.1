using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>索引显示类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexShowType
    {
        #region 索引数据类型
        /// <summary>原生类型</summary>
        Raw,
        /// <summary>图片</summary>
        Image,
        /// <summary>文档</summary>
        Pdf,
        /// <summary>HTML片段</summary>
        Html,
        /// <summary>表格</summary>
        Table,
        /// <summary>图表</summary>
        Chart,
        /// <summary>属性格</summary>
        PropertyGrid,
        /// <summary>音频</summary>
        Audio,
        /// <summary>视频</summary>
        Video,
        /// <summary>综合</summary>
        Mixing,
        #endregion
    }

    /// <summary>元数据</summary>
    public class Metadata : Dictionary<string, object>
    {
        #region 构造函数

        /// <summary>构造函数</summary>
        public Metadata()
        {
        }

        /// <summary>构造函数</summary>
        public Metadata(IDictionary<string, object> values) : base(values)
        {
        }

        #endregion

        #region 数据成员

        /// <summary>索引ID,元数据唯一标识</summary>
        [JsonIgnore]
        public string IIId
        {
            get { return this.GetValueBy(MetadataConsts.IIId).As<string>(); }
            set { base[MetadataConsts.IIId] = value; }
        }

        /// <summary>索引日期</summary>
        [JsonIgnore]
        public DateTime? IndexedDate
        {
            get { return GetDateValue(MetadataConsts.IndexedDate); }
            set { SetDateValue(MetadataConsts.IndexedDate, value); }
        }

        /// <summary>数据源</summary>
        [JsonIgnore]
        public string DSN
        {
            get { return this.GetValueBy(MetadataConsts.DSN).As<string>(); }
            set { base[MetadataConsts.DSN] = value; }
        }
        /// <summary>显示类型</summary>
        [JsonIgnore]
        public string ShowType
        {
            get { return this.GetValueBy(MetadataConsts.ShowType).As<string>(); }
            set { base[MetadataConsts.ShowType] = value; }
        }
        /// <summary>页面数据ID</summary>
        [JsonIgnore]
        public string PageId
        {
            get { return this.GetValueBy(MetadataConsts.PageId).As<string>(); }
            set { base[MetadataConsts.PageId] = value; }
        }

        /// <summary>应用数据ID</summary>
        [JsonIgnore]
        public string DataId
        {
            get { return this.GetValueBy(MetadataConsts.DataId).As<string>(); }
            set { base[MetadataConsts.DataId] = value; }
        }

        /// <summary>资源标识，用于唯一标识数据源的数据</summary>
        [JsonIgnore]
        public string ResourceKey
        {
            get { return this.GetValueBy(MetadataConsts.ResourceKey).As<string>(); }
            //set { base[MetadataConsts.ResourceKey] = value; }
        }

        /// <summary>缩略图</summary>
        [JsonIgnore]
        public string Thumbnail
        {
            get { return this.GetValueBy(MetadataConsts.Thumbnail).As<string>(); }
            set { base[MetadataConsts.Thumbnail] = value; }
        }
        /// <summary>全文</summary>
        [JsonIgnore]
        public string Fulltext
        {
            get { return this.GetValueBy(MetadataConsts.Fulltext).As<string>(); }
            set { base[MetadataConsts.Fulltext] = value; }
        }
        /// <summary>正式标题</summary>
        [JsonIgnore]
        public string Title
        {
            get { return this.GetValueBy(MetadataConsts.Title).As<string>(); }
            //set { base[MetadataConsts.Title] = value; }
        }

        /// <summary>作者</summary>
        [JsonIgnore]
        public string Author
        {
            get { return this.GetValueBy(MetadataConsts.Author).As<string>(); }
            //set { base[MetadataConsts.Author] = value; }
        }

        /// <summary>摘要</summary>
        [JsonIgnore]
        public string Abstract
        {
            get { return this.GetValueBy(MetadataConsts.Abstract).As<string>(); }
            //set { base[MetadataConsts.Abstract] = value; }
        }

        /// <summary>创建日期</summary>
        [JsonIgnore]
        public DateTime? CreatedDate
        {
            get { return GetDateValue(MetadataConsts.CreatedDate); }
            //set { SetDateValue(MetadataConsts.CreatedDate, value); }
        }
        /// <summary>系统</summary>
        [JsonIgnore]
        public string System
        {
            get { return this.GetValueBy(MetadataConsts.System).As<string>(); }
            //set { base[MetadataConsts.System] = value; }
        }
        /// <summary>资源类型</summary>
        [JsonIgnore]
        public string ResourceType
        {
            get { return this.GetValueBy(MetadataConsts.ResourceType).As<string>(); }
            //set { base[MetadataConsts.ResourceType] = value; }
        }

        /// <summary>获取标签值</summary>
        public object GetValue(string tagName, bool ignoreTagTypeCheck = false)
        {
            var tagValue = this.GetValueOrDefaultBy(tagName, null);
            if (ignoreTagTypeCheck) return tagValue;
            if (tagValue == null) return null;
            if (tagValue is DateTime)
            {
                return (DateTime)tagValue;
            }
            return tagValue;
        }

        /// <summary>获取日期标签值</summary>
        private DateTime? GetDateValue(string tagName)
        {
            var tagValue = this.GetValueOrDefaultBy(tagName, null);
            if (tagValue == null) return null;
            return (DateTime)tagValue;
        }

        /// <summary>设置标签值</summary>
        public void SetValue(string tagName, object tagValue, bool ignoreTagTypeCheck = false)
        {
            if (IsNullOrEmpty(tagValue))
            {
                base.Remove(tagName);
                return;
            }
            var tagTokenValue = tagValue.As<JToken>();
            if (tagTokenValue != null)
            {
                switch (tagTokenValue.Type)
                {
                    case JTokenType.Array:
                        tagValue = tagTokenValue.As<JArray>().Cast<JValue>().Select(e => e.Value).ToArray();
                        break;
                    default:
                        tagValue = ((JValue)tagTokenValue).Value;
                        break;
                }
            }
            if (ignoreTagTypeCheck)
            {
                base[tagName] = tagValue;
                return;
            }
            var tag = MetadataDefinitionCollection.Instance[tagName];
            switch ((MetadataTagType)Enum.Parse(typeof(MetadataTagType), tag.Type))
            {
                case MetadataTagType.Date:
                    if (tagValue is DateTime)
                    {
                        //不需要处理
                    }
                    else
                    {
                        tagValue = tagValue.ToString().ToISODateTime();
                    }
                    break;
                case MetadataTagType.StringArray:
                    if (tagValue is IEnumerable && !(tagValue is string))
                    {
                        tagValue = tagValue.As<IEnumerable>().Cast<object>().Select(e => e.ToString()).ToArray();
                    }
                    else
                    {
                        tagValue = new[] { tagValue.ToString() };
                    }
                    break;
                default:
                    if (tagValue is IEnumerable && !(tagValue is string))
                    {
                        tagValue = tagValue.As<IEnumerable>().Cast<object>().First();
                    }
                    tagValue = tagValue.ToString();
                    break;
            }
            base[tagName] = tagValue;
        }

        /// <summary>设置日期标签值</summary>
        private void SetDateValue(string tagName, DateTime? value)
        {
            if (value.HasValue)
            {
                base[tagName] = value.Value;
            }
            else
            {
                base.Remove(tagName);
            }
        }

        /// <summary>清理null或空标签键值</summary>
        public void ClearNullOrEmpty()
        {
            var keys = GetNullOrEmptyKeys();
            foreach (var key in keys)
            {
                base.Remove(key);
            }
        }
        /// <summary>获得null或空标签键</summary>
        public string[] GetNullOrEmptyKeys()
        {
            return this.Where(e => IsNullOrEmpty(e.Value)).Select(e => e.Key).ToArray();
        }
        /// <summary>获得null或空标签键</summary>
        public bool IsNullOrEmpty(object value)
        {
            if (value == null) return true;
            var sValue = value.As<string>();
            if (sValue != null) return sValue.Length == 0;
            var aValue = value.As<object[]>();
            if (aValue != null) return aValue.Length == 0;
            var cValue = value.As<ICollection>();
            if (cValue != null) return cValue.Count == 0;
            return false;
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }

        #endregion
    }
}