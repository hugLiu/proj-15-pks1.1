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

    [Table("Person")]
    public partial class Person : MultiLanguage, IId<int>, ICanLogicalDeleteEntity
    {
        public Person()
        {
            EduHistorys = new HashSet<EduHistory>();
            Honors = new HashSet<Honor>();
        }

        public int Id { get; set; }


        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public DegreeType Degree { get; set; }

        public bool Enabled { get; set; }

        public bool IsDeleted { get; set; }

        public int? Age { get; set; }

        public DateTime? Birth { get; set; }

        public decimal? Salary { get; set; }

        public virtual ICollection<EduHistory> EduHistorys { get; set; }

        [NotMapped]
        public ICollection<Honor> Honors { get; set; }

        public double? Weight { get; set; }

        public double Height { get; set; }

        public double? Health { get; set; }

        public DateTime CreateTime { get; set; }

        public int? LeaderId { get; set; }

        public string Favorites { get; set; }
    }

    public enum DegreeType
    {
        MiddleSchool,

        HighSchool,

        PostGraduate,
        Doctor,
    }
}
