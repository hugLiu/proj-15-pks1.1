using PKS.Utils;
using PKS.Validation;
using PKS.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public class BO2SaveRequest: IParameterValidation
    {
        [CollectionRequired]
        public List<BO2> Values { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
