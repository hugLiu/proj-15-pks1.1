using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddinDemo.Models
{
    public class Stock : IId<int>, IMultiLanguage
    {
        public int Id
        {
            get;
            set;
        }

        public string StockCode { get; set; }

        public DateTime? XchgDate { get; set; }

        public decimal? Price { get; set; }

        [ForeignKey("BillId")]
        public ICollection<Sys_DataLanguage> LangTexts
        {
            get;
            set;
        }
    }

    public class StockModel : IId<int>
    {
        public int Id { get; set; }

        [CatalogExt(DataType = ExtDataType.MultiLanguage)]
        public string StockName { get; set; }

        public string StockCode { get; set; }

        public DateTime? XchgDate { get; set; }

        public decimal? Price { get; set; }
    }
}
