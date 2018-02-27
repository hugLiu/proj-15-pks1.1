namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KFRAGMENT
    {
        public int Id { get; set; }

        public int? KTEMPLATECATALOGUEID { get; set; }

        [Required]
        [StringLength(100)]
        public string TITLE { get; set; }

        [Required]
        [StringLength(1000)]
        public string QUERYPARAMETER { get; set; }

        [StringLength(1000)]
        public string COMPONENTPARAMETER { get; set; }

        //[StringLength(4000)]
        public string HTMLTEXT { get; set; }

        public int KFRAGMENTTYPEID { get; set; }

        [Required]
        [StringLength(50)]
        public string PLACEHOLDERID { get; set; }
        /// <summary>
        /// 知识片段从属于对应标题知识片段的PlaceHolderId
        /// </summary>
        public string MPLACEHOLDERID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KFRAGMENT_TYPE PKS_KFRAGMENT_TYPE { get; set; }

        public virtual PKS_KTEMPLATE_CATALOGUE PKS_KTEMPLATE_CATALOGUE { get; set; }

        public int KTEMPLATEID { get; set; }
    }
}
