using Jurassic.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 记录了增改信息的实体基类
    /// </summary>
    public class CUEntity : ICUEntity
    {
        public int Id
        {
            get;
            set;
        }

        public int CreaterId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreateTime { get; set; }

        public int UpdaterId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }

        [ForeignKey("CreaterId")]
        public virtual UserProfile Creater { get; set; }

        [ForeignKey("UpdaterId")]
        public virtual UserProfile Updater { get; set; }
    }
}
