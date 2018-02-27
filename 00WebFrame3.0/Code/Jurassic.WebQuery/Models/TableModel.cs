using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.WebQuery.Models
{
    //表模型
    public class TableModel
    {
        public string Id { get; set; }
        public string PId { get; set; }
        public string CHName { get; set; }
        public string ENName { get; set; }
        public int Order { get; set; }
        //表包含的属性
        public IList<FieldModel> Fields { get; set; }
    }
}
