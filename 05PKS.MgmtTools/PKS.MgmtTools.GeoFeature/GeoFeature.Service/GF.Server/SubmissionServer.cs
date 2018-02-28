using GF.Server.Factory;
using Jurassic.PKS.Service.GF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Server
{
    public class SubmissionServer : ISubmission
    {
        /// <summary>
        /// 提交3GX数据到主数据库中
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public SubmissionResult Submit(SubmissionInfo info)
        {
            return ObjectCreate<SubmissionResult>.CreateISubmission("SummissionBusiness").Submit(info);
        }
    }
}
