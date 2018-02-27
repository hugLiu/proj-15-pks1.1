using System;
using System.ComponentModel.DataAnnotations;

namespace PKS.DbModels.Portal
{
    public class PKS_SearchHistory
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string SourceId { get; set; }

        [StringLength(128)]
        public string TargetId { get; set; }

        //[Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public DateTime? SourceTime { get; set; }

        public DateTime? TargetTime { get; set; }

        [StringLength(100)]
        public string BrowserName { get; set; }

        [StringLength(20)]
        public string ClientIP { get; set; }

        [StringLength(50)]
        public string SourcePageNameEnum { get; set; }

        [StringLength(50)]
        public string TargetPageNameEnum { get; set; }

        //[Required]
        [StringLength(50)]
        public string SourceWayEnum { get; set; }

        [StringLength(50)]
        public string PageResultsEnum { get; set; }

        public double? RunTime { get; set; }

        [StringLength(200)]
        public string InputWord { get; set; }

        [StringLength(10)]
        public string InputWordTypeEnum { get; set; }

        [StringLength(100)]
        public string BO { get; set; }

        [StringLength(100)]
        public string BOT { get; set; }

        [StringLength(100)]
        public string PT { get; set; }

        [StringLength(100)]
        public string BP { get; set; }

        [StringLength(200)]
        public string Iiid { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string AdapterName { get; set; }

        [StringLength(100)]
        public string ResourcesName { get; set; }

        [StringLength(20)]
        public string ResourcesFormat { get; set; }

        public bool IsDelete { get; set; }
    }
}
