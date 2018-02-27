namespace AddinDemo.Models
{
    using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

    /// <summary>
    /// 教育经历，和主表Person强关联
    /// </summary>
    [Table("EduHistory")]
    public partial class EduHistory : IDetailEntity<Person>
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SchoolName { get; set; }

        public int MasterId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        public virtual Person Master { get; set; }

        public string Special { get; set; }

        public string Subject { get; set; }
    }
}
