using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Jurassic.GeoFeature.Model
{
    public class P
    {
        [DisplayName("名称")]
        public string N { get; set; }
        [DisplayName("类型")]
        public string T { get; set; }
        [DisplayName("量纲")]
        public string D { get; set; }
        [DisplayName("单位")]
        public string U { get; set; }
        [DisplayName("值域")]
        public string V { get; set; }
    }

}
