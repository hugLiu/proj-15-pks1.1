using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public abstract class TypeValue
    {
        public TypeValue()
        { 
        }
        public TypeValue(string type,string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
