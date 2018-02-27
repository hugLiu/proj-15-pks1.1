using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EFProvider
{
    /// <summary>
    /// 时间转换约定
    /// </summary>
    public class DateTime2Convention : Convention
    {
        /// <summary>
        /// 时间转换约定
        /// </summary>
        public DateTime2Convention()
        {
            this.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
