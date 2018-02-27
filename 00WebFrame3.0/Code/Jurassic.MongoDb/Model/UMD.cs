using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class UMD
    {
        public UMD()
        { }
        public UMD(string name,string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; } 
    }
}
