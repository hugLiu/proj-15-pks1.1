/****************************************************************************
* Copyright @ 武汉侏罗纪技术开发有限公司 2017. All rights reserved.
* 
* 文 件 名: Class1
* 创 建 者：zhoush
* 创建日期：2017/7/18 15:46:11
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbModels.Portal
{
    public class PKS_Remark
    {
        public PKS_Remark()
        {
            Thumbups=new HashSet<PKS_Remark_Thumbup>();
        }
        public int Id { get; set; }
        public string IIId { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 评论者
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public ICollection<PKS_Remark_Thumbup> Thumbups { get; set; }
    }
}
