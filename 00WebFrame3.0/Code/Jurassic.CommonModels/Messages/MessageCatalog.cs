using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 初始化消息栏目
    /// </summary>
    public class MessageRoot
    {
        public static MessageRoot Root { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        //[CatalogExt(DataType = ExtDataType.SingleNumber)]
        //public string Channel { get; set; }
    }
}
