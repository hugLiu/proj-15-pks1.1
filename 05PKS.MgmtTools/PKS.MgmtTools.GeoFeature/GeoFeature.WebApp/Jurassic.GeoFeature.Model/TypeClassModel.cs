using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jurassic.GeoFeature.Model
{
    public class TypeClassModel
    {
        private string _classId;

        public string ClassId
        {
            get { return _classId; }
            set { _classId = value; }
        }
        private string _className;
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
    }
}
