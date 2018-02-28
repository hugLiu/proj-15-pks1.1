using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.PKS.Service.Semantics
{

    [Serializable]
    public class WordResult
    {
        /// <summary>
        /// Word
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Part of speech
        /// </summary>
        public string Cc { get; set; }


    }
}
