using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    [Serializable]
    [DataContract]
    public class BO2Collection:List<BO2>
    {
    }
}
