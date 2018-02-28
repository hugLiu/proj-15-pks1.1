using PanGu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.PanGu
{
    public class WordAttributeComparer : IEqualityComparer<WordInfo>
    {
        public bool Equals(WordInfo x, WordInfo y)
        {
            if (x == null)
                return y == null;
            return x.Word == y.Word;
        }

        public int GetHashCode(WordInfo obj)
        {
            if (obj == null)
                return 0;
            return obj.Word.GetHashCode();
        }
    }
}
