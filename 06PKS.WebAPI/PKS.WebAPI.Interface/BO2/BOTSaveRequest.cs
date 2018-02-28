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
    public class BOTSaveRequest: IParameterValidation
    {
        public BOTSaveRequest()
        {
            this.BOTs = new List<BOT>();
        }

        [CollectionRequired]
        public List<BOT> BOTs { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
