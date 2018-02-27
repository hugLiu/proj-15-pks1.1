
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    [BsonIgnoreExtraElements]
    public class CacheInfoItem : MongoObjectId
    {
        public CacheInfoItem()
        {
            this.OperationState = 1;
            this.CompletelyRate = 0;
        }
        public string TaskLogInfoId { get; set; }
        public string AdapterId { get; set; }
        public string AdapterName { get; set; }
        public string DataType { get; set; }
        public DateTime StartDate { get; set; }
        public InfoItem2 InfoItem { get; set; }
        public List<PTItem> PTAnalysisResults { get; set; }
        /// <summary>
        /// 用于缓冲库，表示是否已经进入主库
        /// </summary>
        public bool InInfoItemSource { get; set; }
        /// <summary>
        /// 信息完整率（主要针对必填信息项目）
        /// 必填项为Title、Contributor、Organization、Date、BO、PT、BP、BF
        /// </summary>
        public double CompletelyRate
        {
            get;
            set;
            //get
            //{
            //    int t = this.InfoItem.SMD.Title.Count > 0 ? 1 : 0;
            //    int c = this.InfoItem.SMD.Contributor.Count > 0 ? 1 : 0;
            //    int o = this.InfoItem.SMD.Organization.Count > 0 ? 1 : 0;
            //    int d = this.InfoItem.SMD.Date.Count > 0 ? 1 : 0;
            //    int bo = this.InfoItem.SMD.BO.Count > 0 ? 1 : 0;
            //    int p = string.IsNullOrEmpty(this.InfoItem.SMD.PT) ? 0 : 1;
            //    int bp = string.IsNullOrEmpty(this.InfoItem.SMD.BP) ? 0 : 1;
            //    int bf = this.InfoItem.SMD.BF.Count > 0 ? 1 : 0;
            //    int rate = (t + c + o + d + bo + p + bp + bf) * 100 / 8;
            //    return rate;
            //}
        }
        /// <summary>
        /// 界面操作备用字段
        /// 1：表示当前操作阶段是成果类型采集阶段
        /// 2：表示当前操作阶段是索引数据编辑验证阶段
        /// 3：表示当前操作阶段是提交主库阶段
        /// </summary>
        public int OperationState { get; set; }
    }
}
