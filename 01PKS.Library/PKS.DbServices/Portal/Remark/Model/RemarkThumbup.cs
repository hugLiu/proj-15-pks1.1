/****************************************************************************
* Copyright @ 武汉侏罗纪技术开发有限公司 2017. All rights reserved.
* 
* 文 件 名: Class1
* 创 建 者：zhoush
* 创建日期：2017/7/18 15:46:36
* 功能描述: 
* 
* 修 改 人：    
* 修改时间:     
* 修改日志:    
*
* 审 查 者:     
* 审 查 时 间:  
****************************************************************************/

using System;

namespace PKS.DbServices.Portal.Remark.Model
{
  /// <summary>
  /// 点赞Model
  /// </summary>
    public class RemarkThumbupModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// RemarkId
        /// </summary>
        public int RemarkId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>
        /// LastUpdatedDate
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}
