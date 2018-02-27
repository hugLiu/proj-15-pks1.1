using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class IndexDataQueryModel
    {
        public string TaskLogInfoId { get; set; }
        public string AdapterId { get; set; }
        public string AdapterName { get; set; }
        public string DataType { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 用于缓冲库，表示是否已经进入主库
        /// </summary>
        public bool InInfoItemSource { get; set; }
        /// <summary>
        /// 信息完整率（主要针对必填信息项目）
        /// 必填项为Title、Contributor、Organization、Date、BO、PT、BP、BF
        /// </summary>
        public double CompletelyRate{ get; set; }

        /// <summary>
        /// 界面操作备用字段
        /// 1：表示当前操作阶段是成果类型采集阶段
        /// 2：表示当前操作阶段是索引数据编辑验证阶段
        /// 3：表示当前操作阶段是提交主库阶段
        /// </summary>
        public int OperationState { get; set; }
    }
}
