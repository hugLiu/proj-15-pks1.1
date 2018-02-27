using PKS.DbModels;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>ͼ������</summary>
    [Serializable]
    public class KG_NewTopicModel : KG_NewTopic
    {
        ///<summary>
        /// ����
        ///</summary>
        public string Contents { get; set; }
        ///<summary>
        /// ������
        ///</summary>
        public string CreatedBy { get; set; }
        ///<summary>
        /// ����ʱ��
        ///</summary>
        public string CreatedDate { get; set; }
    }

    /// <summary>ͼ������</summary>
    [Serializable]
    public class KG_TopicModel : KG_NewTopicModel
    {
        ///<summary>
        /// ������������
        ///</summary>
        public List<TreeNode> PublicCatalogs { get; set; }
        ///<summary>
        /// ������������
        ///</summary>
        public List<TreeNode> PrivateCatalogs { get; set; }
    }
}
