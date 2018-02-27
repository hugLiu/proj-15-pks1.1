namespace AddinDemo.Models
{
    using Jurassic.CommonModels.EntityBase;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// �ö�����һ��������û��ǿ�������ӱ����
    /// </summary>
    [Table("Honor")]
    public partial class Honor : IAttachmentEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string HonorName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GetDate { get; set; }

        public int BillId
        {
            get;
            set;
        }

        public string ModuleCode
        {
            get;
            set;
        }
    }
}
