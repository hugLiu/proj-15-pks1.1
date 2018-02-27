namespace AddinDemo.Models
{
    using Jurassic.AppCenter;
    using Jurassic.CommonModels.Articles;
    using Jurassic.CommonModels.EntityBase;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [CatalogExt(EntityType = typeof(Person))]
    public partial class PersonModel : IId<int> 
    {
        public PersonModel()
        {
            EduHistorys = new List<EduHistoryModel>();
            Honors = new List<HonorModel>();
        }

        [CatalogExt(DataSourceType = ExtDataSourceType.Hidden)]
        public int Id { get; set; }

        [CatalogExt(DataType = ExtDataType.MultiLanguage)]
        public string Name { get; set; }

        [StringLength(50)]
        [CatalogExt(DataSourceType=ExtDataSourceType.UserDefine)]
        public string Country { get; set; }

        [CatalogExt(LinkedProperty = "Country", Editable=false)]
        [StringLength(50)]
        public string City { get; set; }

        public DegreeType Degree { get; set; }

        public bool Enabled { get; set; }

        [CatalogExt(DataType = ExtDataType.DateAndTime)]
        public DateTime CreateTime { get; set; }

        [CatalogExt(DataType=ExtDataType.UserId)]
        public int? LeaderId { get; set; }

        [CatalogExt(MaxValue = 150, MinValue = 0)]
        public int? Age { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? Birth { get; set; }

        public double? Weight { get; set; }

        public double Height { get; set; }

        [CatalogExt(DataType=ExtDataType.Percent)]
        public double? Health { get; set; }

        [CatalogExt(DataType=ExtDataType.ButtonEdit)]
        public string Favorites { get; set; }

        public virtual IList<EduHistoryModel> EduHistorys { get; set; }

        public virtual IList<HonorModel> Honors { get; set; }
    }
}
