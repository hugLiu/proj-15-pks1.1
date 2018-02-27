using System;
using System.Collections.Generic;

namespace PKS.DbServices.Portal.Remark.Model
{
    /// <summary>
    /// 评论Model
    /// </summary>
    public class RemarkModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 成果Id
        /// </summary>
        public string IIId { get; set; }
    
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 评论名
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最近修改人
        /// </summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>
        /// 最近修改日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string UserPhotoUrl { get; set; }

        /// <summary>
        /// 点赞信息
        /// </summary>
        public List<RemarkThumbupModel> Thumbups { get; set; }
      
    }
}
