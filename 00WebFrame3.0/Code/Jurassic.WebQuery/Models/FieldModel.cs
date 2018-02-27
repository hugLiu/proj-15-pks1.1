using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.WebQuery.Models
{
    //表属性模型
    public class FieldModel
    {
        public string Id { get; set; }
        public string TableId { get; set; }
        public bool IsKey { get; set; }
        public string TableENName { get; set; }
        public string CHName { get; set; }
        public string ENName { get; set; }
        public int Order { get; set; }
    }
}
