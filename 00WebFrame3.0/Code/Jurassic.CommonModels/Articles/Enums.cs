using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 文章扩展属性的数据类型
    /// </summary>
    public enum ExtDataType
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        SingleLineText,

        /// <summary>
        /// 多行文本
        /// </summary>
        MultiLineText,


        /// <summary>
        /// Html文本
        /// </summary>
        Html,

        /// <summary>
        /// 整数
        /// </summary>
        SingleNumber,

        /// <summary>
        /// 小数
        /// </summary>
        FloatNumber,

        /// <summary>
        /// 货币
        /// </summary>
        Currency,

        /// <summary>
        /// 是/否
        /// </summary>
        Bool,

        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateAndTime,

        /// <summary>
        /// 图形
        /// </summary>
        Image,

        /// <summary>
        /// 文件
        /// </summary>
        File,

        /// <summary>
        /// 地址
        /// </summary>
        Address,

        /// <summary>
        /// 不包含日期的时间数据
        /// </summary>
        Time,

        /// <summary>
        /// 百分比
        /// </summary>
        Percent,

        /// <summary>
        /// 指定数据是用户ID
        /// </summary>
        UserId,

        /// <summary>
        /// 一个ButtonEdit控件，需要用户在前台自定义按钮事件
        /// </summary>
        ButtonEdit,

        /// <summary>
        /// 表示该属性支持多语言
        /// </summary>
        MultiLanguage,

        /// <summary>
        /// 初始化时的默认值，因为0=SingleLineText
        /// 所以用这个，该值在分析属性类型后，会被其他值代替
        /// </summary>
        Auto,
    }

    /// <summary>
    /// 用以描述扩展属性数据源类型的枚举
    /// </summary>
    public enum ExtDataSourceType
    {
        /// <summary>
        /// 默认值，不属任何列表类型
        /// </summary>
        None,

        /// <summary>
        /// 单项选择列表
        /// </summary>
        DirectList,

        /// <summary>
        /// 多项选择列表
        /// </summary>
        MultipleList,

        /// <summary>
        /// SQL查询
        /// </summary>
        SqlQuery,

        /// <summary>
        /// SQL查询+多选
        /// </summary>
        SqlQueryMultipleList,

        /// <summary>
        /// 树控件
        /// </summary>
        TreeSelect,

        /// <summary>
        /// 用户自定义数据源
        /// </summary>
        UserDefine,

        /// <summary>
        /// 隐藏，不显示控件，只生成一个隐藏域
        /// </summary>
        Hidden,

        /// <summary>
        /// 用户自定义控件
        /// </summary>
        Custom,
    }

    /// <summary>
    /// 内容的状态值.它们通常组合使用。
    /// 仅凭以下定义的状态，可能不能覆盖今后内容状态管理的所有可能性，
    /// 但可以根据这些状态的组合搭配出其他状态，并且可以把组合状态定义为一些静态变量方便使用。
    /// </summary>
    public static class ArticleState
    {
        /// <summary>
        /// 默认无状态 (0)
        /// </summary>
        public const int None = 0;


        /// <summary>
        /// 表示内容已正式发布(0x2)
        /// </summary>
        public const int Published = 2;

        /// <summary>
        /// 表示是新的 (0x4)
        /// </summary>
        public const int New = 4;
        /// <summary>
        /// 推荐 (0x8)
        /// </summary>
        public const int Recommended = 8;

        /// <summary>
        /// 置顶 (0x10 = 16)
        /// </summary>
        public const int SetTop = 16;

        /// <summary>
        /// 只读的，表示此属性不能由用户端改写，只能由程序改写 （0x20 = 32)
        /// </summary>
        public const int ReadOnly = 32;

        /// <summary>
        /// 表示回复 (0x40 - 64)
        /// </summary>
        public const int Reply = 64;

        /// <summary>
        /// 表示同意(0x-80 - 128)
        /// </summary>
        public const int Agree = 128;

        /// <summary>
        /// 分隔符，这个状态目前没有明确意义 0x100 = 256 
        /// </summary>
        public const int Separator = 256;

        /// <summary>
        /// 表示不可更改，由程序自动生成，且是系统运行所必须 (0x200 = 512)
        /// </summary>
        public const int Static = 512;

        /// <summary>
        /// 共享 (0x400 = 1024)
        /// </summary>
        public const int Shared = 1024;

        /// <summary>
        /// 未完成 (0x800)
        /// </summary>
        public const int Incomplete = 2048;
    }

    /// <summary>
    /// 标识文章之间的联系的常量类
    /// </summary>
    public static class ArticleRelationType
    {
        public const int None = 0;

        public const int Thumbnail = 1;

        public const int Product = 2;

        public const int Download = 4;

        public const int Solution = 8;

        public const int Case = 16;

        public const int Attachment = 32;

        public const int Reply = 64;
    }

    /// <summary>
    /// 目录所有者类型
    /// </summary>
    public enum CatalogOwnerType
    {
        User,
        Department,
        System,
        Application
    }
}
