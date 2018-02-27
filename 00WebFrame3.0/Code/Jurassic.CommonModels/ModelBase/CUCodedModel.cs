using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    public abstract class CUCodedModel : CUModel, ICodedModel
    {
        public string Code
        {
            get;
            set;
        }
    }
}
