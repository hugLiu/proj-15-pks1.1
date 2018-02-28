using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.Core.Common
{
    public static class DateTimeExtension
    {
        public static string ToMonthDay(this DateTime dt)
        {
            return dt.ToString("MM-dd");
        }
        public static string ToEsDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:MM:ss.ms");
        }
    }
}
