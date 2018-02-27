using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class Contributor : TypeValue
    {
        public Contributor()
        {
        }
        public Contributor(string value)
        {
            this.Value = value;
        }

        public Contributor(string type, string value)
            : base(type, value)
        {
        }
    }
}
