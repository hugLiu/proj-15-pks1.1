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

    [CatalogExt(EntityType=typeof(EduHistory))]
    public partial class EduHistoryModel : IId<int>
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SchoolName { get; set; }

        [Column(TypeName = "date")]
        [CatalogExt(DataType = ExtDataType.Date)]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [CatalogExt(DataSource = "机械=1;电子=2;建筑=3;医药=4", AllowNull=false)]
        public string Special { get; set; }

        [CatalogExt(LinkedProperty = "Special")]
        public string Subject { get; set; }



    }
}
