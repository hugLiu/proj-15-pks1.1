using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    public class JDataEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public JDataEventArgs(T t)
        {
            Data = t;
        }
    }
}
