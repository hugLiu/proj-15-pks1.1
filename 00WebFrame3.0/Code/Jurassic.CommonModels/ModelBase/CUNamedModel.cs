using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    public abstract class CUNamedModel : CUModel,INamedModel
    {
        public string Name
        {
            get;
            set;
        }
    }
}
