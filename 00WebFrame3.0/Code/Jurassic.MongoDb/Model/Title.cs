using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class Title:TypeValue
    {
        public Title()
        { 
        }
        public Title(string type, string value):base(type,value)
        {
        }
    }
}
