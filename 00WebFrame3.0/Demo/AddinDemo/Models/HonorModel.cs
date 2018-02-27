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

    [CatalogExt(EntityType = typeof(Honor))]
    public partial class HonorModel : IId<int>
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string HonorName { get; set; }

        [Column(TypeName = "date")]
        [CatalogExt(DataType=ExtDataType.Date)]
        public DateTime? GetDate { get; set; }
    }
}
