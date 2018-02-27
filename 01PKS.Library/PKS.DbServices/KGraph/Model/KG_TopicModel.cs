using PKS.DbModels;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>图谱主题</summary>
    [Serializable]
    public class KG_NewTopicModel : KG_NewTopic
    {
        ///<summary>
        /// 内容
        ///</summary>
        public string Contents { get; set; }
        ///<summary>
        /// 创建人
        ///</summary>
        public string CreatedBy { get; set; }
        ///<summary>
        /// 创建时间
        ///</summary>
        public string CreatedDate { get; set; }
    }

    /// <summary>图谱主题</summary>
    [Serializable]
    public class KG_TopicModel : KG_NewTopicModel
    {
        ///<summary>
        /// 公共分类数组
        ///</summary>
        public List<TreeNode> PublicCatalogs { get; set; }
        ///<summary>
        /// 公共分类数组
        ///</summary>
        public List<TreeNode> PrivateCatalogs { get; set; }
    }
}
