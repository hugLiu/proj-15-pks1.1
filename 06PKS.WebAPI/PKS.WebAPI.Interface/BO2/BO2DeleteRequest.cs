using PKS.Utils;
using PKS.Validation;
using PKS.Web;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PKS.WebAPI.Models
{
    public class BO2DeleteRequest: IParameterValidation
    {
        [DataMember(Name = "bot", IsRequired = true)]
        public string BOT { get; set; }

        [DataMember(Name = "bos", IsRequired = true)]
        [CollectionRequired]
        public List<string> BOs { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}