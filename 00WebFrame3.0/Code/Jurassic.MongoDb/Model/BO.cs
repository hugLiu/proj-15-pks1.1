using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class BO : TypeValue
    {
        public BO()
        {
        }
        public BO(string type, string value)
            : base(type, value)
        {
        }
    }
}
